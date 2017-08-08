﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Job.TransactionHandler.Core;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;
using Lykke.Job.TransactionHandler.Core.Domain.CashOperations;
using Lykke.Job.TransactionHandler.Core.Domain.Clients.Core.Clients;
using Lykke.Job.TransactionHandler.Core.Domain.Offchain;
using Lykke.Job.TransactionHandler.Core.Services.Offchain;
using Lykke.Job.TransactionHandler.Queues.Common;
using Lykke.Job.TransactionHandler.Queues.Models;
using Lykke.Service.OperationsRepository.Client.Abstractions.CashOperations;
using TransferEventClient = Lykke.Service.OperationsRepository.AutorestClient.Models.TransferEvent;
using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Queues
{
    public class TransferQueue : RabbitQueue
    {
        private readonly ILog _log;
        private readonly IBitcoinCommandSender _bitcoinCommandSender;
        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly IBitCoinTransactionsRepository _bitCoinTransactionsRepository;
        private readonly ITransferOperationsRepositoryClient _transferEventsRepositoryClient;
        private readonly IOffchainRequestService _offchainRequestService;
        private readonly IClientSettingsRepository _clientSettingsRepository;
        private readonly IOffchainIgnoreRepository _offchainIgnoreRepository;
        private readonly IMapper _mapper;

        public TransferQueue(AppSettings.RabbitMqSettings config, ILog log,
            IBitcoinCommandSender bitcoinCommandSender,
            ITransferOperationsRepositoryClient transferEventsRepositoryClient,
            IWalletCredentialsRepository walletCredentialsRepository,
            IBitCoinTransactionsRepository bitCoinTransactionsRepository, 
            IOffchainRequestService offchainRequestService,
            IClientSettingsRepository clientSettingsRepository, 
            IOffchainIgnoreRepository offchainIgnoreRepository,
            IMapper mapper)
            : base(config.ExternalHost, config.Port,
                  config.ExchangeTransfer, "transactions.transfer",
                  config.Username, config.Password, log)
        {
            _log = log;
            _bitcoinCommandSender = bitcoinCommandSender;
            _transferEventsRepositoryClient = transferEventsRepositoryClient;
            _walletCredentialsRepository = walletCredentialsRepository;
            _bitCoinTransactionsRepository = bitCoinTransactionsRepository;
            _offchainRequestService = offchainRequestService;
            _clientSettingsRepository = clientSettingsRepository;
            _offchainIgnoreRepository = offchainIgnoreRepository;
            _mapper = mapper;
        }

        public override async Task<bool> ProcessMessage(string message)
        {
            var queueMessage = JsonConvert
                .DeserializeObject<TransferQueueMessage>(message);
            var amount = queueMessage.Amount.ParseAnyDouble();

            //Get client wallets
            var toWallet = await _walletCredentialsRepository.GetAsync(queueMessage.ToClientid);
            var fromWallet = await _walletCredentialsRepository.GetAsync(queueMessage.FromClientId);

            //Register transfer events
            var destTransferEvent = TransferEvent.CreateNew(queueMessage.ToClientid,
                toWallet.MultiSig, null,
                queueMessage.AssetId, amount, queueMessage.Id,
                toWallet.Address, toWallet.MultiSig, state: TransactionStates.SettledOffchain);
            var destTransfer =
                await _transferEventsRepositoryClient.RegisterAsync(_mapper.Map<TransferEventClient>(destTransferEvent));

            var sourceTransferEvent = TransferEvent.CreateNew(queueMessage.FromClientId,
                fromWallet.MultiSig, null,
                queueMessage.AssetId, -amount, queueMessage.Id,
                fromWallet.Address, fromWallet.MultiSig, state: TransactionStates.SettledOffchain);
            var sourceTransfer =
                await _transferEventsRepositoryClient.RegisterAsync(
                    _mapper.Map<TransferEventClient>(sourceTransferEvent));

            //Craete or Update transfer context
            var transaction = await _bitCoinTransactionsRepository.FindByTransactionIdAsync(queueMessage.Id);
            if (transaction == null)
            {
                await _log.WriteWarningAsync(nameof(TransferQueue), nameof(ProcessMessage), message, "unkown transaction");
                return false;
            }

            var contextData = transaction.GetContextData<TransferContextData>();

            if (contextData == null)
            {
                contextData = TransferContextData
                    .Create(queueMessage.FromClientId, new TransferContextData.TransferModel
                    {
                        ClientId = queueMessage.ToClientid
                    }, new TransferContextData.TransferModel
                    {
                        ClientId = queueMessage.FromClientId
                    });
            }

            contextData.Transfers[0].OperationId = destTransfer.Id;
            contextData.Transfers[1].OperationId = sourceTransfer.Id;

            var contextJson = contextData.ToJson();
            var cmd = new TransferCommand
            {
                Amount = amount,
                AssetId = queueMessage.AssetId,
                Context = contextJson,
                SourceAddress = fromWallet.MultiSig,
                DestinationAddress = toWallet.MultiSig,
                TransactionId = Guid.Parse(queueMessage.Id)
            };

            await _bitCoinTransactionsRepository.UpdateAsync(transaction.TransactionId,
                cmd.ToJson(), contextJson, "");

            if (await _clientSettingsRepository.IsOffchainClient(queueMessage.ToClientid))
            {
                if (!await _offchainIgnoreRepository.IsIgnored(queueMessage.ToClientid))
                {
                    try
                    {
                        await _offchainRequestService.CreateOffchainRequestAndNotify(transaction.TransactionId, queueMessage.ToClientid, queueMessage.AssetId, (decimal)amount, null, OffchainTransferType.CashinToClient);
                    }
                    catch (Exception)
                    {
                        await _log.WriteWarningAsync(nameof(TransferQueue), nameof(ProcessMessage), "", $"Transfer already exists {transaction.TransactionId}");
                    }
                }
            }
            else
                await _bitcoinCommandSender.SendCommand(cmd);



            return true;
        }
    }
}