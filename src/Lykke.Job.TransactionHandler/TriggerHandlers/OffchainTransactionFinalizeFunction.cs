﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.TransactionHandler.Core;
using Lykke.Job.TransactionHandler.Core.Domain.Assets;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;
using Lykke.Job.TransactionHandler.Core.Domain.CashOperations;
using Lykke.Job.TransactionHandler.Core.Domain.Clients;
using Lykke.Job.TransactionHandler.Core.Domain.MarginTrading;
using Lykke.Job.TransactionHandler.Core.Domain.Offchain;
using Lykke.Job.TransactionHandler.Core.Domain.PaymentSystems;
using Lykke.Job.TransactionHandler.Core.Services.AppNotifications;
using Lykke.Job.TransactionHandler.Core.Services.BitCoin.BitCoinApi;
using Lykke.Job.TransactionHandler.Core.Services.ChronoBank;
using Lykke.Job.TransactionHandler.Core.Services.MarginTrading;
using Lykke.Job.TransactionHandler.Core.Services.Messages.Email;
using Lykke.Job.TransactionHandler.Core.Services.Offchain;
using Lykke.Job.TransactionHandler.Core.Services.Quanta;
using Lykke.Job.TransactionHandler.Core.Services.SolarCoin;
using Lykke.Job.TransactionHandler.Resources;
using Lykke.Job.TransactionHandler.Services.Notifications;
using Lykke.JobTriggers.Triggers.Attributes;
using Lykke.Service.Assets.Client.Custom;
using Lykke.Service.ExchangeOperations.Contracts;
using Lykke.Service.PersonalData.Contract;

namespace Lykke.Job.TransactionHandler.TriggerHandlers
{
    public class OffchainTransactionFinalizeFunction
    {
        private readonly IBitCoinTransactionsRepository _bitCoinTransactionsRepository;
        private readonly ICashOperationsRepository _cashOperationsRepository;
        private readonly ICashOutAttemptRepository _cashOutAttemptRepository;
        private readonly IClientTradesRepository _clientTradesRepository;
        private readonly IClientAccountsRepository _clientAccountsRepository;
        private readonly IPersonalDataService _personalDataService;
        private readonly IOffchainTransferRepository _offchainTransferRepository;
        private readonly ITransferEventsRepository _transferEventsRepository;
        private readonly IOffchainRequestService _offchainRequestService;
        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly IBitcoinApiClient _bitcoinApiClient;
        private readonly IOffchainRequestRepository _offchainRequestRepository;
        private readonly CachedDataDictionary<string, IAssetSetting> _assetSettings;
        private readonly IPaymentTransactionsRepository _paymentTransactionsRepository;
        private readonly IAppNotifications _appNotifications;
        private readonly ICachedAssetsService _assetsService;

        private readonly IMarginDataServiceResolver _marginDataServiceResolver;
        private readonly IMarginTradingPaymentLogRepository _marginTradingPaymentLog;


        private readonly ISrvEmailsFacade _srvEmailsFacade;
        private readonly IExchangeOperationsService _exchangeOperationsService;
        private readonly SrvSlackNotifications _srvSlackNotifications;
        private readonly ILog _log;

        private readonly IChronoBankService _chronoBankService;
        private readonly ISrvSolarCoinHelper _srvSolarCoinHelper;
        private readonly IQuantaService _quantaService;

        public OffchainTransactionFinalizeFunction(
            IBitCoinTransactionsRepository bitCoinTransactionsRepository,
            ILog log,
            ICashOperationsRepository cashOperationsRepository,
            IExchangeOperationsService exchangeOperationsService,
            SrvSlackNotifications srvSlackNotifications,
            ICashOutAttemptRepository cashOutAttemptRepository,
            ISrvEmailsFacade srvEmailsFacade,
            IClientTradesRepository clientTradesRepository,
            IClientAccountsRepository clientAccountsRepository,
            IPersonalDataService personalDataService,
            IOffchainTransferRepository offchainTransferRepository,
            IChronoBankService chronoBankService,
            ISrvSolarCoinHelper srvSolarCoinHelper,
            ITransferEventsRepository transferEventsRepository,
            IQuantaService quantaService,
            IOffchainRequestService offchainRequestService,
            IWalletCredentialsRepository walletCredentialsRepository,
            IBitcoinApiClient bitcoinApiClient,
            IOffchainRequestRepository offchainRequestRepository,
            CachedDataDictionary<string, IAssetSetting> assetSettings,
            IMarginDataServiceResolver marginDataServiceResolver,
            IMarginTradingPaymentLogRepository marginTradingPaymentLog,
            IPaymentTransactionsRepository paymentTransactionsRepository,
            IAppNotifications appNotifications,
            ICachedAssetsService assetsService)
        {
            _bitCoinTransactionsRepository = bitCoinTransactionsRepository;
            _log = log;
            _cashOperationsRepository = cashOperationsRepository;
            _exchangeOperationsService = exchangeOperationsService;
            _srvSlackNotifications = srvSlackNotifications;
            _cashOutAttemptRepository = cashOutAttemptRepository;
            _srvEmailsFacade = srvEmailsFacade;
            _clientTradesRepository = clientTradesRepository;
            _clientAccountsRepository = clientAccountsRepository;
            _personalDataService = personalDataService;
            _offchainTransferRepository = offchainTransferRepository;
            _chronoBankService = chronoBankService;
            _srvSolarCoinHelper = srvSolarCoinHelper;
            _transferEventsRepository = transferEventsRepository;
            _quantaService = quantaService;
            _offchainRequestService = offchainRequestService;
            _walletCredentialsRepository = walletCredentialsRepository;
            _bitcoinApiClient = bitcoinApiClient;
            _offchainRequestRepository = offchainRequestRepository;
            _assetSettings = assetSettings;

            _marginDataServiceResolver = marginDataServiceResolver;
            _marginTradingPaymentLog = marginTradingPaymentLog;
            _paymentTransactionsRepository = paymentTransactionsRepository;
            _appNotifications = appNotifications;
            _assetsService = assetsService;
        }

        [QueueTrigger("offchain-finalization", notify: true, maxDequeueCount: 1, maxPollingIntervalMs: 100)]
        public async Task Process(OffchainFinalizetionMessage message)
        {
            var transfer = await _offchainTransferRepository.GetTransfer(message.TransferId);

            if (transfer.Type == OffchainTransferType.HubCashout)
                return;

            var transactionId =
                transfer.Type == OffchainTransferType.FromClient || transfer.Type == OffchainTransferType.FromHub
                    ? transfer.OrderId
                    : transfer.Id;

            var transaction = await _bitCoinTransactionsRepository.SaveResponseAndHashAsync(transactionId, null, message.TransactionHash);

            if (transaction == null)
            {
                await _log.WriteWarningAsync(nameof(OffchainTransactionFinalizeFunction), nameof(Process), $"Transaction: {transactionId}, client: {message.ClientId}, hash: {message.TransactionHash}, transfer: {message.TransferId}", "unkown transaction");
                return;
            }

            switch (transaction.CommandType)
            {
                case BitCoinCommands.Issue:
                    await FinalizeIssue(transaction);
                    break;
                case BitCoinCommands.CashOut:
                    await FinalizeCashOut(transaction, transfer);
                    break;
                case BitCoinCommands.Transfer:
                    await FinalizeTransfer(transaction, transfer);
                    break;
                case BitCoinCommands.SwapOffchain:
                    await FinalizeSwap(transaction, transfer);
                    break;
                default:
                    await _log.WriteWarningAsync(nameof(OffchainTransactionFinalizeFunction), nameof(Process), $"Transaction: {transactionId}, client: {message.ClientId}, hash: {message.TransactionHash}, transfer: {message.TransferId}", "unkown command type");
                    break;
            }
        }

        private async Task FinalizeIssue(IBitcoinTransaction transaction)
        {
            var contextData = transaction.GetContextData<IssueContextData>();

            await _cashOperationsRepository.SetIsSettledAsync(contextData.ClientId, contextData.CashOperationId, true);
        }

        private Task FinalizeTransfer(IBitcoinTransaction transaction, IOffchainTransfer transfer)
        {
            var contextData = transaction.GetContextData<TransferContextData>();

            switch (contextData.TransferType)
            {
                case TransferType.ToMarginAccount:
                    return FinalizeTransferToMargin(contextData, transfer);
                case TransferType.Common:
                    return FinalizeCommonTransfer(transaction, contextData);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task FinalizeTransferToMargin(TransferContextData context, IOffchainTransfer transfer)
        {
            var sourceTransferContext = context.Transfers.FirstOrDefault(x => x.ClientId == transfer.ClientId);
            var destTransferContext = context.Transfers.FirstOrDefault(x => x.ClientId != transfer.ClientId);

            if (sourceTransferContext?.Actions?.UpdateMarginBalance == null)
                throw new Exception();

            await _exchangeOperationsService.FinishTransferAsync(
                transfer.Id,
                sourceTransferContext.ClientId,
                destTransferContext.ClientId,
                (double)transfer.Amount,
                transfer.AssetId);

            var marginDataService = _marginDataServiceResolver.Resolve(false);

            var depositToMarginResult = await marginDataService.DepositToAccount(transfer.ClientId,
                sourceTransferContext.Actions.UpdateMarginBalance.AccountId,
                sourceTransferContext.Actions.UpdateMarginBalance.Amount,
                MarginPaymentType.Transfer);

            if (depositToMarginResult)
                await _marginTradingPaymentLog.CreateAsync(MarginTradingPaymentLog.CreateOk(transfer.ClientId,
                    sourceTransferContext.Actions.UpdateMarginBalance.AccountId, DateTime.UtcNow,
                    sourceTransferContext.Actions.UpdateMarginBalance.Amount, transfer.ExternalTransferId));
            else
            {
                var errorLog = MarginTradingPaymentLog.CreateError(transfer.ClientId,
                    sourceTransferContext.Actions.UpdateMarginBalance.AccountId, DateTime.UtcNow,
                    sourceTransferContext.Actions.UpdateMarginBalance.Amount, "Error deposit to margin account");

                await _marginTradingPaymentLog.CreateAsync(errorLog);
                await _srvSlackNotifications.SendNotification(ChannelTypes.MarginTrading, errorLog.ToJson(), "Transaction handler");
            }

        }

        private async Task FinalizeCommonTransfer(IBitcoinTransaction transaction, TransferContextData contextData)
        {
            foreach (var transfer in contextData.Transfers)
            {
                await _transferEventsRepository.SetIsSettledIfExistsAsync(transfer.ClientId, transfer.OperationId, true);

                var clientData = await _personalDataService.GetAsync(transfer.ClientId);

                if (transfer.Actions?.CashInConvertedOkEmail != null)
                {
                    await
                        _srvEmailsFacade.SendTransferCompletedEmail(clientData.Email, clientData.FullName,
                            transfer.Actions.CashInConvertedOkEmail.AssetFromId, transfer.Actions.CashInConvertedOkEmail.AmountFrom,
                            transfer.Actions.CashInConvertedOkEmail.AmountLkk, transfer.Actions.CashInConvertedOkEmail.Price, transaction.BlockchainHash);
                }

                if (transfer.Actions?.SendTransferEmail != null)
                {
                    await
                        _srvEmailsFacade.SendDirectTransferCompletedEmail(clientData.Email, clientData.FullName,
                            transfer.Actions.SendTransferEmail.AssetId, transfer.Actions.SendTransferEmail.Amount,
                            transaction.BlockchainHash);
                }

                if (transfer.Actions?.PushNotification != null)
                {
                    var clientAcc = await _clientAccountsRepository.GetByIdAsync(transfer.ClientId);
                    var asset = await _assetsService.TryGetAssetAsync(transfer.Actions.PushNotification.AssetId);

                    await _appNotifications.SendAssetsCreditedNotification(new[] { clientAcc.NotificationsId },
                            transfer.Actions.PushNotification.Amount, transfer.Actions.PushNotification.AssetId,
                            string.Format(TextResources.CreditedPushText, transfer.Actions.PushNotification.Amount.GetFixedAsString(asset.Accuracy),
                                transfer.Actions.PushNotification.AssetId));
                }
            }

            await _paymentTransactionsRepository.SetStatus(transaction.TransactionId, PaymentStatus.NotifyProcessed);
        }

        private async Task FinalizeCashOut(IBitcoinTransaction transaction, IOffchainTransfer offchainTransfer)
        {
            var amount = Math.Abs((double)offchainTransfer.Amount);

            var data = await _exchangeOperationsService.FinishCashOutAsync(transaction.TransactionId, offchainTransfer.ClientId, (double)offchainTransfer.Amount, offchainTransfer.AssetId);

            await CreateHubCashoutIfNeed(offchainTransfer);

            if (!data.IsOk())
            {
                await _log.WriteWarningAsync("CashOutController", "CashOut", data.ToJson(), "ME operation failed");
                await _srvSlackNotifications.SendNotification(ChannelTypes.Errors, $"Cashout failed in ME, client: {offchainTransfer.ClientId}, transfer: {transaction.TransactionId}, ME code result: {data.Code}");
            }

            var contextData = transaction.GetContextData<CashOutContextData>();

            var swiftData = contextData.AddData?.SwiftData;
            if (swiftData != null)
            {
                await _cashOutAttemptRepository.SetIsSettledOffchain(contextData.ClientId, swiftData.CashOutRequestId);
            }
            else
            {
                if (offchainTransfer.AssetId == LykkeConstants.SolarAssetId)
                {
                    await PostSolarCashOut(offchainTransfer.ClientId, contextData.Address, amount, transaction.TransactionId);
                }
                else if (offchainTransfer.AssetId == LykkeConstants.ChronoBankAssetId)
                {
                    await PostChronoBankCashOut(contextData.Address, amount, transaction.TransactionId);
                }
                else if (offchainTransfer.AssetId == LykkeConstants.QuantaAssetId)
                {
                    await PostQuantaCashOut(contextData.Address, amount, transaction.TransactionId);
                }
                else
                {
                    var clientData = await _personalDataService.GetAsync(contextData.ClientId);
                    await _srvEmailsFacade.SendNoRefundOCashOutMail(clientData.Email, contextData.Amount, contextData.AssetId, transaction.BlockchainHash);
                }
            }
        }

        private async Task FinalizeSwap(IBitcoinTransaction transaction, IOffchainTransfer offchainTransfer)
        {
            var contextData = transaction.GetContextData<SwapOffchainContextData>();

            foreach (var trade in contextData.Operations)
            {
                if (trade.TransactionId == offchainTransfer.Id)
                {
                    if (string.IsNullOrWhiteSpace(trade.ClientTradeId))
                    {
                        await _log.WriteWarningAsync(nameof(OffchainTransactionFinalizeFunction), nameof(FinalizeSwap), contextData.ToJson(), "Client trade id is missing");
                    }
                    else
                    {
                        await _clientTradesRepository.SetIsSettledAsync(trade.ClientId, trade.ClientTradeId, true);
                    }

                    break;
                }
            }
        }

        private Task PostChronoBankCashOut(string address, double amount, string txId)
        {
            return _chronoBankService.SendCashOutRequest(txId, address, amount);
        }

        private Task PostQuantaCashOut(string address, double amount, string txId)
        {
            return _quantaService.SendCashOutRequest(txId, address, amount);
        }

        private async Task PostSolarCashOut(string clientId, string address, double amount, string txId)
        {
            var slrAddress = new SolarCoinAddress(address);
            var clientAcc = _clientAccountsRepository.GetByIdAsync(clientId);

            var sendEmailTask = _srvEmailsFacade.SendSolarCashOutCompletedEmail((await clientAcc).Email, slrAddress.Value, amount);
            var solarRequestTask = _srvSolarCoinHelper.SendCashOutRequest(txId, slrAddress, amount);

            await Task.WhenAll(sendEmailTask, solarRequestTask);
        }

        private async Task CreateHubCashoutIfNeed(IOffchainTransfer offchainTransfer)
        {
            try
            {
                var client = await _walletCredentialsRepository.GetAsync(offchainTransfer.ClientId);

                var currentRequests = (await _offchainRequestRepository.GetRequestsForClient(offchainTransfer.ClientId)).ToList();
                var currentChannels = await _bitcoinApiClient.Balances(client.MultiSig);

                var hasBtcRequest =
                    currentRequests.FirstOrDefault(x => x.AssetId == LykkeConstants.BitcoinAssetId &&
                                                        x.TransferType == OffchainTransferType.HubCashout) != null;
                var hasLkkRequest =
                    currentRequests.FirstOrDefault(x => x.AssetId == LykkeConstants.LykkeAssetId &&
                                                        x.TransferType == OffchainTransferType.HubCashout) != null;

                var btcSetting = await GetAssetSetting(LykkeConstants.BitcoinAssetId);
                var lkkSetting = await GetAssetSetting(LykkeConstants.LykkeAssetId);

                var btcHubAmount = !currentChannels.HasError &&
                                   currentChannels.Balances.ContainsKey(LykkeConstants.BitcoinAssetId) &&
                                   currentChannels.Balances[LykkeConstants.BitcoinAssetId].Actual
                                    ? currentChannels.Balances[LykkeConstants.BitcoinAssetId].HubAmount
                                    : 0;

                var lkkHubAmount = !currentChannels.HasError &&
                                    currentChannels.Balances.ContainsKey(LykkeConstants.LykkeAssetId) &&
                                    currentChannels.Balances[LykkeConstants.LykkeAssetId].Actual
                                    ? currentChannels.Balances[LykkeConstants.LykkeAssetId].HubAmount
                                    : 0;

                var needBtcCashout = offchainTransfer.AssetId != LykkeConstants.BitcoinAssetId && btcHubAmount > btcSetting.Dust && !hasBtcRequest;
                var needLkkCashout = offchainTransfer.AssetId != LykkeConstants.LykkeAssetId && lkkHubAmount > lkkSetting.Dust && !hasLkkRequest;

                await _offchainRequestService.CreateHubCashoutRequests(offchainTransfer.ClientId, needBtcCashout ? btcHubAmount : 0, needLkkCashout ? lkkHubAmount : 0);
            }
            catch (Exception e)
            {
                await _log.WriteErrorAsync(nameof(OffchainTransactionFinalizeFunction), nameof(CreateHubCashoutIfNeed), $"{offchainTransfer.ClientId}", e);
            }
        }

        private async Task<IAssetSetting> GetAssetSetting(string asset)
        {
            var setting = await _assetSettings.GetItemAsync(asset) ?? await _assetSettings.GetItemAsync(LykkeConstants.DefaultAssetSetting);
            if (setting == null)
                throw new Exception($"Setting is not found for {asset}");
            return setting;
        }
    }

    public class OffchainFinalizetionMessage
    {
        public string ClientId { get; set; }
        public string TransferId { get; set; }
        public string TransactionHash { get; set; }
    }
}
