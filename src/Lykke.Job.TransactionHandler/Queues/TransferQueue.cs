﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Log;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;
using Lykke.Job.TransactionHandler.Core.Domain.CashOperations;
using Lykke.Job.TransactionHandler.Core.Domain.Clients;
using Lykke.Job.TransactionHandler.Core.Domain.Clients.Core.Clients;
using Lykke.Job.TransactionHandler.Core.Domain.Offchain;
using Lykke.Job.TransactionHandler.Core.Services.BitCoin;
using Lykke.Job.TransactionHandler.Core.Services.Offchain;
using Lykke.Job.TransactionHandler.Queues.Models;
using Lykke.Service.OperationsRepository.Client.Abstractions.CashOperations;
using TransferEventClient = Lykke.Service.OperationsRepository.AutorestClient.Models.TransferEvent;
using Newtonsoft.Json;
using Lykke.Job.TransactionHandler.Services;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;

namespace Lykke.Job.TransactionHandler.Queues
{
    public class TransferQueue : IQueueSubscriber
    {
        private const string QueueName = "transactions.transfer";

        private readonly ILog _log;
        private readonly IBitcoinCommandSender _bitcoinCommandSender;
        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly IBitCoinTransactionsRepository _bitCoinTransactionsRepository;
        private readonly ITransferOperationsRepositoryClient _transferEventsRepositoryClient;
        private readonly IOffchainRequestService _offchainRequestService;
        private readonly IClientSettingsRepository _clientSettingsRepository;
        private readonly IMapper _mapper;
        private readonly IBitcoinTransactionService _bitcoinTransactionService;
        private readonly IClientAccountsRepository _clientAccountsRepository;

        private readonly AppSettings.RabbitMqSettings _rabbitConfig;
        private RabbitMqSubscriber<TransferQueueMessage> _subscriber;

        public TransferQueue(AppSettings.RabbitMqSettings config, ILog log,
            IBitcoinCommandSender bitcoinCommandSender,
            ITransferOperationsRepositoryClient transferEventsRepositoryClient,
            IWalletCredentialsRepository walletCredentialsRepository,
            IBitCoinTransactionsRepository bitCoinTransactionsRepository, 
            IOffchainRequestService offchainRequestService,
            IClientSettingsRepository clientSettingsRepository,
            IBitcoinTransactionService bitcoinTransactionService,
            IClientAccountsRepository clientAccountsRepository,
            IMapper mapper)
        {
            _rabbitConfig = config;
            _log = log;
            _bitcoinCommandSender = bitcoinCommandSender;
            _transferEventsRepositoryClient = transferEventsRepositoryClient;
            _walletCredentialsRepository = walletCredentialsRepository;
            _bitCoinTransactionsRepository = bitCoinTransactionsRepository;
            _offchainRequestService = offchainRequestService;
            _clientSettingsRepository = clientSettingsRepository;
            _mapper = mapper;
            _bitcoinTransactionService = bitcoinTransactionService;
            _clientAccountsRepository = clientAccountsRepository;
        }

        public void Start()
        {
            var settings = new RabbitMqSubscriptionSettings
            {
                ConnectionString = _rabbitConfig.ConnectionString,
                QueueName = QueueName,
                ExchangeName = _rabbitConfig.ExchangeTransfer,
                DeadLetterExchangeName = $"{_rabbitConfig.ExchangeTransfer}.dlx",
                RoutingKey = "",
                IsDurable = true
            };

            try
            {
                _subscriber = new RabbitMqSubscriber<TransferQueueMessage>(settings, new DeadQueueErrorHandlingStrategy(_log, settings))
                    .SetMessageDeserializer(new JsonMessageDeserializer<TransferQueueMessage>())
                    .SetMessageReadStrategy(new MessageReadQueueStrategy())
                    .Subscribe(ProcessMessage)
                    .CreateDefaultBinding()
                    .SetLogger(_log)
                    .Start();
            }
            catch (Exception ex)
            {
                _log.WriteErrorAsync(nameof(TransferQueue), nameof(Start), null, ex).Wait();
                throw;
            }
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }

        public async Task<bool> ProcessMessage(TransferQueueMessage queueMessage)
        {
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
                await _log.WriteWarningAsync(nameof(TransferQueue), nameof(ProcessMessage), queueMessage.ToJson(), "unkown transaction");
                return false;
            }

            var contextData = await _bitcoinTransactionService.GetTransactionContext<TransferContextData>(transaction.TransactionId);

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

            await _bitCoinTransactionsRepository.UpdateAsync(transaction.TransactionId, cmd.ToJson(), null, "");

            await _bitcoinTransactionService.SetTransactionContext(transaction.TransactionId, contextData);

            if (await _clientSettingsRepository.IsOffchainClient(queueMessage.ToClientid))
            {
                if (!await _clientAccountsRepository.IsTrusted(queueMessage.ToClientid))
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

        public void Dispose()
        {
            Stop();
        }
    }
}