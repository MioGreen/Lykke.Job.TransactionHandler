﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.TransactionHandler.Core;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;
using Lykke.Job.TransactionHandler.Core.Domain.Blockchain;
using Lykke.Job.TransactionHandler.Core.Domain.CashOperations;
using Lykke.Job.TransactionHandler.Core.Domain.Clients;
using Lykke.Job.TransactionHandler.Core.Domain.Clients.Core.Clients;
using Lykke.Job.TransactionHandler.Core.Domain.Ethereum;
using Lykke.Job.TransactionHandler.Core.Domain.Exchange;
using Lykke.Job.TransactionHandler.Core.Domain.Offchain;
using Lykke.Job.TransactionHandler.Core.Services.AppNotifications;
using Lykke.Job.TransactionHandler.Core.Services.Ethereum;
using Lykke.Job.TransactionHandler.Core.Services.Offchain;
using Lykke.Job.TransactionHandler.Queues.Common;
using Lykke.Job.TransactionHandler.Queues.Models;
using Lykke.Job.TransactionHandler.Resources;
using Lykke.Job.TransactionHandler.Services;
using Lykke.Service.Assets.Client.Custom;
using Lykke.Service.Assets.Client.Models;
using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Queues
{
    public class LimitTradeQueue : RabbitQueue
    {
#if DEBUG
        private const string QueueName = "transactions.limit-trades-dev";
        private const bool QueueDurable = false;
        private const bool QueueAutoDelete = true;
#else
        private const string QueueName = "transactions.limit-trades";
        private const bool QueueDurable = true;
        private const bool QueueAutoDelete = false;
#endif

        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly IBitCoinTransactionsRepository _bitcoinTransactionsRepository;
        private readonly IOffchainRequestService _offchainRequestService;
        private readonly IOffchainOrdersRepository _offchainOrdersRepository;
        private readonly IClientSettingsRepository _clientSettingsRepository;
        private readonly IClientAccountsRepository _clientAccountsRepository;
        private readonly IAppNotifications _appNotifications;
        private readonly IEthereumTransactionRequestRepository _ethereumTransactionRequestRepository;
        private readonly ISrvEthereumHelper _srvEthereumHelper;
        private readonly IBcnClientCredentialsRepository _bcnClientCredentialsRepository;
        private readonly AppSettings.EthereumSettings _settings;
        private readonly IEthClientEventLogs _ethClientEventLogs;
        private readonly ILog _log;
        private readonly ICachedAssetsService _assetsService;
        private readonly CachedDataDictionary<string, IOffchainIgnore> _offchainIgnoreDictionary;
        private readonly ILimitOrdersRepository _limitOrdersRepository;
        private readonly IClientTradesRepository _clientTradesRepository;
        private readonly ILimitTradeEventsRepository _limitTradeEventsRepository;
        private readonly IClientCacheRepository _clientCacheRepository;

        public LimitTradeQueue(
            AppSettings.RabbitMqSettings config,
            ILog log,
            IWalletCredentialsRepository walletCredentialsRepository,
            IBitCoinTransactionsRepository bitcoinTransactionsRepository,
            IOffchainRequestService offchainRequestService,
            IEthereumTransactionRequestRepository ethereumTransactionRequestRepository,
            ISrvEthereumHelper srvEthereumHelper,
            ICachedAssetsService assetsService,
            IBcnClientCredentialsRepository bcnClientCredentialsRepository,
            AppSettings.EthereumSettings settings,
            IEthClientEventLogs ethClientEventLogs,
            CachedDataDictionary<string, IOffchainIgnore> offchainIgnoreDictionary, ILimitOrdersRepository limitOrdersRepository, IClientTradesRepository clientTradesRepository, ILimitTradeEventsRepository limitTradeEventsRepository, IClientSettingsRepository clientSettingsRepository, IAppNotifications appNotifications, IClientAccountsRepository clientAccountsRepository, IOffchainOrdersRepository offchainOrdersRepository, IClientCacheRepository clientCacheRepository)
            : base(config.ExternalHost, config.Port,
                config.ExchangeLimit, QueueName,
                config.Username, config.Password, log, QueueDurable, QueueAutoDelete, false)
        {
            _walletCredentialsRepository = walletCredentialsRepository;
            _bitcoinTransactionsRepository = bitcoinTransactionsRepository;
            _offchainRequestService = offchainRequestService;
            _ethereumTransactionRequestRepository = ethereumTransactionRequestRepository;
            _srvEthereumHelper = srvEthereumHelper;
            _assetsService = assetsService;
            _bcnClientCredentialsRepository = bcnClientCredentialsRepository;
            _settings = settings;
            _ethClientEventLogs = ethClientEventLogs;
            _log = log;
            _offchainIgnoreDictionary = offchainIgnoreDictionary;
            _limitOrdersRepository = limitOrdersRepository;
            _clientTradesRepository = clientTradesRepository;
            _limitTradeEventsRepository = limitTradeEventsRepository;
            _clientSettingsRepository = clientSettingsRepository;
            _appNotifications = appNotifications;
            _clientAccountsRepository = clientAccountsRepository;
            _offchainOrdersRepository = offchainOrdersRepository;
            _clientCacheRepository = clientCacheRepository;
        }

        public override async Task<bool> ProcessMessage(string message)
        {
            var tradeItem = JsonConvert
                .DeserializeObject<LimitQueueItem>(message);

            foreach (var limitOrderWithTrades in tradeItem.Orders)
            {
                try
                {
                    var meOrder = limitOrderWithTrades.Order;

                    await _limitOrdersRepository.CreateOrUpdateAsync(meOrder);

                    var status = (OrderStatus)Enum.Parse(typeof(OrderStatus), meOrder.Status);

                    var aggregated = AggregateSwaps(limitOrderWithTrades.Trades);

                    switch (status)
                    {
                        case OrderStatus.InOrderBook:
                        case OrderStatus.Cancelled:
                            await CreateEvent(limitOrderWithTrades, status);
                            break;
                        case OrderStatus.Processing:
                        case OrderStatus.Matched:
                            var trades = await ProcessTrades(aggregated, limitOrderWithTrades);
                            await SendMoney(trades, aggregated, meOrder);
                            break;
                        case OrderStatus.Dust:
                        case OrderStatus.NoLiquidity:
                        case OrderStatus.NotEnoughFunds:
                        case OrderStatus.ReservedVolumeGreaterThanBalance:
                        case OrderStatus.UnknownAsset:
                        case OrderStatus.LeadToNegativeSpread:
                            await _log.WriteInfoAsync(nameof(LimitTradeQueue), nameof(ProcessMessage), limitOrderWithTrades.ToJson(), "order rejected");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(OrderStatus));
                    }

                    if (status == OrderStatus.Cancelled || status == OrderStatus.Matched)
                        await ReturnRemainingVolume(limitOrderWithTrades);

                    await UpdateCache(meOrder);

                    await SendPush(aggregated, meOrder, status);
                }
                catch (Exception e)
                {
                    await _log.WriteErrorAsync(nameof(LimitTradeQueue), nameof(ProcessMessage), limitOrderWithTrades.ToJson(), e);
                }
            }

            return true;
        }

        private async Task<IClientTrade[]> ProcessTrades(List<AggregatedTransfer> operations, LimitQueueItem.LimitOrderWithTrades limitOrderWithTrades)
        {
            if (limitOrderWithTrades.Trades.Count == 0)
                return new IClientTrade[0];

            var walletCredsClientA = await _walletCredentialsRepository.GetAsync(limitOrderWithTrades.Trades[0].ClientId);
            var walletCredsClientB = await _walletCredentialsRepository.GetAsync(limitOrderWithTrades.Trades[0].OppositeClientId);

            var trades = limitOrderWithTrades.ToDomainOffchain(limitOrderWithTrades.Order.Id, walletCredsClientA, walletCredsClientB);

            await _clientTradesRepository.SaveAsync(trades);

            var currentTransaction = await _bitcoinTransactionsRepository.FindByTransactionIdAsync(limitOrderWithTrades.Order.Id);

            var contextData = new SwapOffchainContextData();

            if (!string.IsNullOrWhiteSpace(currentTransaction?.ContextData))
                contextData = JsonConvert.DeserializeObject<SwapOffchainContextData>(currentTransaction?.ContextData);

            foreach (var operation in operations.Where(x => x.ClientId == limitOrderWithTrades.Order.ClientId))
            {
                var trade = trades.FirstOrDefault(x => x.ClientId == operation.ClientId && x.AssetId == operation.AssetId && Math.Abs(x.Amount - (double)operation.Amount) < 0.00000001);

                contextData.Operations.Add(new SwapOffchainContextData.Operation()
                {
                    TransactionId = operation.TransferId,
                    Amount = operation.Amount,
                    ClientId = operation.ClientId,
                    AssetId = operation.AssetId,
                    ClientTradeId = trade?.Id
                });
            }

            if (currentTransaction == null)
                await _bitcoinTransactionsRepository.CreateAsync(limitOrderWithTrades.Order.Id,
                    BitCoinCommands.SwapOffchain, "", contextData.ToJson(), "");
            else
                await _bitcoinTransactionsRepository.UpdateAsync(limitOrderWithTrades.Order.Id, "",
                    contextData.ToJson(), "");

            return trades;
        }

        private async Task SendMoney(IClientTrade[] clientTrades, IEnumerable<AggregatedTransfer> aggregatedTransfers, ILimitOrder order)
        {
            if (await IsClientTrusted(order.ClientId))
                return;

            var clientId = order.ClientId;

            var executed = aggregatedTransfers.FirstOrDefault(x => x.ClientId == clientId && x.Amount > 0);

            var asset = await _assetsService.TryGetAssetAsync(executed.AssetId);
            if (asset.Blockchain == Blockchain.Ethereum)
            {
                await ProcessEthBuy(executed, asset, clientTrades, order.Id);
                return;
            }

            await _offchainRequestService.CreateOffchainRequestAndNotify(executed.TransferId, clientId,
                executed.AssetId, executed.Amount, order.Id, OffchainTransferType.FromHub);
        }

        private async Task SendPush(IEnumerable<AggregatedTransfer> aggregatedTransfers, ILimitOrder order, OrderStatus status)
        {
            if (await IsClientTrusted(order.ClientId))
                return;

            var clientId = order.ClientId;
            var type = order.Volume > 0 ? OrderType.Buy : OrderType.Sell;
            var typeString = type.ToString().ToLower();
            var assetPair = await _assetsService.TryGetAssetPairAsync(order.AssetPairId);
            var neededAsset = type == OrderType.Buy ? assetPair.QuotingAssetId : assetPair.BaseAssetId;
            var receivedAsset = type == OrderType.Buy ? assetPair.BaseAssetId : assetPair.QuotingAssetId;
            var volume = Math.Abs(order.Volume);
            var remainingVolume = Math.Abs(order.RemainingVolume);
            var executedSum = Math.Abs(aggregatedTransfers.Where(x => x.ClientId == clientId && x.AssetId == receivedAsset)
                                .Select(x => x.Amount)
                                .DefaultIfEmpty(0)
                                .Sum());

            string msg;
            
            switch (status)
            {
                case OrderStatus.InOrderBook:
                    msg = string.Format(TextResources.LimitOrderStarted, typeString, order.AssetPairId, volume, order.Price, neededAsset);
                    break;
                case OrderStatus.Cancelled:
                    msg = string.Format(TextResources.LimitOrderCancelled, typeString, order.AssetPairId, volume, order.Price, neededAsset);
                    break;
                case OrderStatus.Processing:
                    msg = string.Format(TextResources.LimitOrderPartiallyExecuted, typeString, order.AssetPairId, remainingVolume, order.Price, neededAsset, executedSum, receivedAsset);
                    break;
                case OrderStatus.Matched:
                    msg = string.Format(TextResources.LimitOrderExecuted, typeString, order.AssetPairId, remainingVolume, order.Price, neededAsset, executedSum, receivedAsset);
                    break;
                case OrderStatus.Dust:
                case OrderStatus.NoLiquidity:
                case OrderStatus.NotEnoughFunds:
                case OrderStatus.ReservedVolumeGreaterThanBalance:
                case OrderStatus.UnknownAsset:
                case OrderStatus.LeadToNegativeSpread:
                    msg = string.Format(TextResources.LimitOrderRejected, typeString, order.AssetPairId, volume, order.Price, neededAsset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(OrderStatus));
            }

            var pushSettings = await _clientSettingsRepository.GetSettings<PushNotificationsSettings>(clientId);
            if (pushSettings.Enabled)
            {
                var clientAcc = await _clientAccountsRepository.GetByIdAsync(clientId);

                await _appNotifications.SendLimitOrderNotification(new[] { clientAcc.NotificationsId }, msg, type, status);
            }
        }

        private async Task ReturnRemainingVolume(LimitQueueItem.LimitOrderWithTrades limitOrderWithTrades)
        {
            if (await IsClientTrusted(limitOrderWithTrades.Order.ClientId))
                return;

            var order = limitOrderWithTrades.Order;
            var offchainOrder = await _offchainOrdersRepository.GetOrder(order.Id);

            var type = order.Volume > 0 ? OrderType.Buy : OrderType.Sell;
            var assetPair = await _assetsService.TryGetAssetPairAsync(order.AssetPairId);

            if (type == OrderType.Buy)
            {
                var neededAsset = assetPair.QuotingAssetId;
                var initial = offchainOrder.ReservedVolume;

                var trades = await _clientTradesRepository.GetByOrderAsync(order.Id);

                var executed = trades.Where(x => x.AssetId == neededAsset && x.ClientId == order.ClientId)
                    .Select(x => x.Amount).DefaultIfEmpty(0).Sum();

                var returnAmount = Math.Max(0, initial - Math.Abs((decimal)executed));

                if (returnAmount > 0)
                    await _offchainRequestService.CreateOffchainRequestAndNotify(Guid.NewGuid().ToString(), order.ClientId,
                        neededAsset, returnAmount, order.Id, OffchainTransferType.FromHub);
            }
            else
            {
                var neededAsset = assetPair.BaseAssetId;
                var remainigVolume = Math.Abs((decimal)order.RemainingVolume);

                if (remainigVolume > 0)
                {
                    await _offchainRequestService.CreateOffchainRequestAndNotify(Guid.NewGuid().ToString(), order.ClientId,
                        neededAsset, remainigVolume, order.Id, OffchainTransferType.FromHub);
                }
            }
        }

        private async Task CreateEvent(LimitQueueItem.LimitOrderWithTrades limitOrderWithTrades, OrderStatus status)
        {
            if (await IsClientTrusted(limitOrderWithTrades.Order.ClientId))
                return;

            var order = limitOrderWithTrades.Order;
            var type = order.Volume > 0 ? OrderType.Buy : OrderType.Sell;
            var assetPair = await _assetsService.TryGetAssetPairAsync(order.AssetPairId);
            await _limitTradeEventsRepository.CreateEvent(order.Id, order.ClientId, type, order.Volume,
                assetPair?.BaseAssetId, order.AssetPairId, order.Price, status);
        }

        private async Task UpdateCache(IOrderBase meOrder)
        {
            if (await IsClientTrusted(meOrder.ClientId))
                return;

            var count = (await _limitOrdersRepository.GetActiveOrdersAsync(meOrder.ClientId)).Count();

            await _clientCacheRepository.UpdateLimitOrdersCount(meOrder.ClientId, count);
        }

        private async Task<bool> IsClientTrusted(string clientId)
        {
            return await _offchainIgnoreDictionary.GetItemAsync(clientId) != null;
        }

        private async Task ProcessEthBuy(AggregatedTransfer operation, IAsset asset, IClientTrade[] clientTrades, string orderId)
        {
            string errMsg = string.Empty;
            var transferId = Guid.NewGuid();

            try
            {
                var toAddress = await _bcnClientCredentialsRepository.GetClientAddress(operation.ClientId);

                await _ethereumTransactionRequestRepository.InsertAsync(new EthereumTransactionRequest
                {
                    AddressTo = toAddress,
                    AssetId = asset.Id,
                    ClientId = operation.ClientId,
                    Id = transferId,
                    OperationIds =
                        clientTrades.Where(x => x.ClientId == operation.ClientId && x.Amount > 0)
                            .Select(x => x.Id)
                            .ToArray(),
                    OperationType = OperationType.Trade,
                    OrderId = orderId,
                    Volume = operation.Amount
                });

                var res = await _srvEthereumHelper.SendTransferAsync(transferId, string.Empty, asset,
                    _settings.HotwalletAddress, toAddress, operation.Amount);

                if (res.HasError)
                {
                    errMsg = res.Error.ToJson();

                    await _log.WriteWarningAsync(nameof(TradeQueue), nameof(ProcessEthGuaranteeTransfer), errMsg, string.Empty);
                }
            }
            catch (Exception e)
            {
                await _log.WriteErrorAsync(nameof(TradeQueue), nameof(ProcessEthGuaranteeTransfer), e.Message, e);

                errMsg = $"{e.GetType()}\n{e.Message}";
            }

            if (!string.IsNullOrEmpty(errMsg))
            {
                await _ethClientEventLogs.WriteEvent(operation.ClientId, Event.Error, new
                {
                    Info = $"{asset.Id} was not transferred to client",
                    RequestId = transferId,
                    Operation = operation.ToJson(),
                    Error = errMsg
                }.ToJson());
            }
        }

        private async Task<bool> ProcessEthGuaranteeTransfer(IEthereumTransactionRequest ethereumTxRequest, List<AggregatedTransfer> operations, IClientTrade[] clientTrades)
        {
            string errMsg = string.Empty;
            IAsset asset = await _assetsService.TryGetAssetAsync(ethereumTxRequest.AssetId);
            try
            {
                var fromAddress = await _bcnClientCredentialsRepository.GetClientAddress(ethereumTxRequest.ClientId);
                var clientEthSellOperation =
                    operations.First(x => x.Amount < 0 && x.ClientId == ethereumTxRequest.ClientId);
                var change = ethereumTxRequest.Volume - Math.Abs(clientEthSellOperation.Amount);

                EthereumResponse<OperationResponse> res;
                var minAmountForAsset = (decimal)Math.Pow(10, -asset.Accuracy);
                if (change > 0 && Math.Abs(change) >= minAmountForAsset)
                {
                    res = await _srvEthereumHelper.SendTransferWithChangeAsync(change,
                        ethereumTxRequest.SignedTransfer.Sign, ethereumTxRequest.SignedTransfer.Id,
                        asset, fromAddress, _settings.HotwalletAddress, ethereumTxRequest.Volume);
                }
                else
                {
                    res = await _srvEthereumHelper.SendTransferAsync(ethereumTxRequest.SignedTransfer.Id, ethereumTxRequest.SignedTransfer.Sign,
                        asset, fromAddress, _settings.HotwalletAddress, ethereumTxRequest.Volume);
                }

                if (res.HasError)
                {
                    errMsg = res.Error.ToJson();
                    await _log.WriteWarningAsync(nameof(TradeQueue), nameof(ProcessEthGuaranteeTransfer), errMsg, string.Empty);
                }

                ethereumTxRequest.OperationIds =
                    clientTrades.Where(x => x.ClientId == ethereumTxRequest.ClientId && x.Amount < 0)
                        .Select(x => x.Id)
                        .ToArray();
                await _ethereumTransactionRequestRepository.UpdateAsync(ethereumTxRequest);
            }
            catch (Exception e)
            {
                await _log.WriteErrorAsync(nameof(TradeQueue), nameof(ProcessEthGuaranteeTransfer), e.Message, e);

                errMsg = $"{e.GetType()}\n{e.Message}";
            }

            if (!string.IsNullOrEmpty(errMsg))
            {
                await _ethClientEventLogs.WriteEvent(ethereumTxRequest.ClientId, Event.Error, new
                {
                    Info = $"Guarantee transfer of {asset.Id} failed",
                    Operations = operations.ToJson(),
                    RequestId = ethereumTxRequest.Id,
                    Error = errMsg
                }.ToJson());
                return false;
            }

            return true;
        }

        private List<AggregatedTransfer> AggregateSwaps(IEnumerable<LimitQueueItem.LimitTradeInfo> trades)
        {
            var list = new List<AggregatedTransfer>();

            foreach (var swap in trades)
            {
                var amount1 = Convert.ToDecimal(swap.Volume);
                var amount2 = Convert.ToDecimal(swap.OppositeVolume);

                AddAmount(list, swap.ClientId, swap.Asset, -amount1);
                AddAmount(list, swap.OppositeClientId, swap.Asset, amount1);

                AddAmount(list, swap.OppositeClientId, swap.OppositeAsset, -amount2);
                AddAmount(list, swap.ClientId, swap.OppositeAsset, amount2);
            }

            return list;
        }

        private void AddAmount(ICollection<AggregatedTransfer> list, string client, string asset, decimal amount)
        {
            var client1 = list.FirstOrDefault(x => x.ClientId == client && x.AssetId == asset);
            if (client1 != null)
                client1.Amount += amount;
            else
                list.Add(new AggregatedTransfer
                {
                    Amount = amount,
                    ClientId = client,
                    AssetId = asset,
                    TransferId = Guid.NewGuid().ToString()
                });
        }

        private class AggregatedTransfer
        {
            public string ClientId { get; set; }

            public string AssetId { get; set; }

            public decimal Amount { get; set; }

            public string TransferId { get; set; }
        }
    }
}
