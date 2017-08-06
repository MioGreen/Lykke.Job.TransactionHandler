using System;
using System.Threading.Tasks;
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
using Lykke.Job.TransactionHandler.Services;
using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Queues
{
    public class TransferQueue : RabbitQueue
    {
        private readonly ILog _log;
        private readonly IBitcoinCommandSender _bitcoinCommandSender;
        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly IBitCoinTransactionsRepository _bitCoinTransactionsRepository;
        private readonly ITransferEventsRepository _transferEventsRepository;
        private readonly IOffchainRequestService _offchainRequestService;
        private readonly IClientSettingsRepository _clientSettingsRepository;
        private readonly IOffchainIgnoreRepository _offchainIgnoreRepository;

        public TransferQueue(AppSettings.RabbitMqSettings config, ILog log,
            IBitcoinCommandSender bitcoinCommandSender,
            ITransferEventsRepository transferEventsRepository,
            IWalletCredentialsRepository walletCredentialsRepository,
            IBitCoinTransactionsRepository bitCoinTransactionsRepository, IOffchainRequestService offchainRequestService, IClientSettingsRepository clientSettingsRepository, IOffchainIgnoreRepository offchainIgnoreRepository)
            : base(config.ExternalHost, config.Port,
                  config.ExchangeTransfer, "transactions.transfer",
                  config.Username, config.Password, log)
        {
            _log = log;
            _bitcoinCommandSender = bitcoinCommandSender;
            _transferEventsRepository = transferEventsRepository;
            _walletCredentialsRepository = walletCredentialsRepository;
            _bitCoinTransactionsRepository = bitCoinTransactionsRepository;
            _offchainRequestService = offchainRequestService;
            _clientSettingsRepository = clientSettingsRepository;
            _offchainIgnoreRepository = offchainIgnoreRepository;
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
            var destTransfer =
                await
                    _transferEventsRepository.RegisterAsync(
                        TransferEvent.CreateNew(queueMessage.ToClientid,
                        toWallet.MultiSig, null,
                        queueMessage.AssetId, amount, queueMessage.Id,
                        toWallet.Address, toWallet.MultiSig, state: TransactionStates.SettledOffchain));

            var sourceTransfer =
                await
                    _transferEventsRepository.RegisterAsync(
                        TransferEvent.CreateNew(queueMessage.FromClientId,
                        fromWallet.MultiSig, null,
                        queueMessage.AssetId, -amount, queueMessage.Id,
                        fromWallet.Address, fromWallet.MultiSig, state: TransactionStates.SettledOffchain));

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