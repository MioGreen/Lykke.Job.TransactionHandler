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
using Lykke.Job.TransactionHandler.Core.Domain.Ethereum;
using Lykke.Job.TransactionHandler.Core.Domain.Exchange;
using Lykke.Job.TransactionHandler.Core.Domain.Offchain;
using Lykke.Job.TransactionHandler.Core.Services.Ethereum;
using Lykke.Job.TransactionHandler.Core.Services.Offchain;
using Lykke.Job.TransactionHandler.Queues.Common;
using Lykke.Job.TransactionHandler.Queues.Models;
using Lykke.Job.TransactionHandler.Services;
using Lykke.Service.Assets.Client.Custom;
using Lykke.Service.Assets.Client.Models;
using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Queues
{
    public class TradeQueue : RabbitQueue
    {
#if DEBUG
        private const string QueueName = "transactions.trades-dev";
        private const bool QueueDurable = false;
        private const bool QueueAutoDelete = true;
#else
        private const string QueueName = "transactions.trades";
        private const bool QueueDurable = true;
        private const bool QueueAutoDelete = false;
#endif

        private readonly IBitcoinCommandSender _bitcoinCommandSender;
        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly IBitCoinTransactionsRepository _bitcoinTransactionsRepository;
        private readonly IMarketOrdersRepository _marketOrdersRepository;
        private readonly IClientTradesRepository _clientTradesRepository;
        private readonly IOffchainRequestService _offchainRequestService;
        private readonly IOffchainOrdersRepository _offchainOrdersRepository;
        private readonly IOffchainTransferRepository _offchainTransferRepository;
        private readonly IOffchainIgnoreRepository _offchainIgnoreRepository;
        private readonly IEthereumTransactionRequestRepository _ethereumTransactionRequestRepository;
        private readonly ISrvEthereumHelper _srvEthereumHelper;
        private readonly IBcnClientCredentialsRepository _bcnClientCredentialsRepository;
        private readonly AppSettings.EthereumSettings _settings;
        private readonly IEthClientEventLogs _ethClientEventLogs;
        private readonly ILog _log;
        private readonly ICachedAssetsService _assetsService;

        public TradeQueue(
            AppSettings.RabbitMqSettings config, 
            ILog log,
            IBitcoinCommandSender bitcoinCommandSender,
            IWalletCredentialsRepository walletCredentialsRepository,
            IBitCoinTransactionsRepository bitcoinTransactionsRepository,
            IMarketOrdersRepository marketOrdersRepository,
            IClientTradesRepository clientTradesRepository, 
            IOffchainRequestService offchainRequestService,
            IOffchainOrdersRepository offchainOrdersRepository, 
            IOffchainTransferRepository offchainTransferRepository,
            IOffchainIgnoreRepository offchainIgnoreRepository, 
            IEthereumTransactionRequestRepository ethereumTransactionRequestRepository,
            ISrvEthereumHelper srvEthereumHelper, 
            ICachedAssetsService assetsService,
            IBcnClientCredentialsRepository bcnClientCredentialsRepository, 
            AppSettings.EthereumSettings settings,
            IEthClientEventLogs ethClientEventLogs)
            : base(config.ExternalHost, config.Port,
                config.ExchangeSwap, QueueName,
                config.Username, config.Password, log, QueueDurable, QueueAutoDelete)
        {
            _bitcoinCommandSender = bitcoinCommandSender;
            _walletCredentialsRepository = walletCredentialsRepository;
            _bitcoinTransactionsRepository = bitcoinTransactionsRepository;
            _marketOrdersRepository = marketOrdersRepository;
            _clientTradesRepository = clientTradesRepository;
            _offchainRequestService = offchainRequestService;
            _offchainOrdersRepository = offchainOrdersRepository;
            _offchainTransferRepository = offchainTransferRepository;
            _offchainIgnoreRepository = offchainIgnoreRepository;
            _ethereumTransactionRequestRepository = ethereumTransactionRequestRepository;
            _srvEthereumHelper = srvEthereumHelper;
            _assetsService = assetsService;
            _bcnClientCredentialsRepository = bcnClientCredentialsRepository;
            _settings = settings;
            _ethClientEventLogs = ethClientEventLogs;
            _log = log;
        }

        public override async Task<bool> ProcessMessage(string message)
        {
            var tradeItem = JsonConvert
                .DeserializeObject<TradeQueueItem>(message);

            await _marketOrdersRepository.CreateAsync(tradeItem.Order);

            if (!tradeItem.Order.Status.Equals("matched", StringComparison.OrdinalIgnoreCase))
            {
                await _log.WriteInfoAsync(nameof(TradeQueue), nameof(ProcessMessage), message, "Message processing being aborted, due to order status is not matched. Order was saved");
                return true;
            }

            // offchain operation
            var offchainOrder = await _offchainOrdersRepository.GetOrder(tradeItem.Order.ExternalId);
            if (offchainOrder != null)
                return await ProcessOffchainMessage(offchainOrder, tradeItem);

            var idx = 0;
            foreach (var trade in tradeItem.Trades)
            {
                trade.Timestamp = trade.Timestamp.AddMilliseconds(idx++);

                var transactionId = Guid.NewGuid();

                var walletCredsMarket = await _walletCredentialsRepository.GetAsync(trade.MarketClientId);
                var walletCredsLimit = await _walletCredentialsRepository.GetAsync(trade.LimitClientId);

                var tradeRecords = await _clientTradesRepository.SaveAsync(trade.GetTradeRecords(tradeItem.Order, transactionId.ToString(),
                    walletCredsMarket, walletCredsLimit));

                SwapContextData contextData = PrepareContextData(tradeRecords);

                await _bitcoinTransactionsRepository.CreateAsync(transactionId.ToString(), BitCoinCommands.Swap, "", contextData.ToJson(), "");

                var amount1 = trade.MarketVolume;
                var amount2 = trade.LimitVolume;

                //Send to bitcoin
                await _bitcoinCommandSender.SendCommand(new SwapCommand
                {
                    MultisigCustomer1 = walletCredsMarket.MultiSig,
                    Asset1 = trade.MarketAsset,
                    Amount1 = amount1,
                    MultisigCustomer2 = walletCredsLimit.MultiSig,
                    Asset2 = trade.LimitAsset,
                    Amount2 = amount2,
                    Context = contextData.ToJson(),
                    TransactionId = transactionId
                });
            }

            return true;
        }

        private static SwapContextData PrepareContextData(IClientTrade[] tradeRecords)
        {
            return new SwapContextData
            {
                Trades = tradeRecords
                    .Select(t => new SwapContextData.TradeModel
                    {
                        ClientId = t.ClientId,
                        TradeId = t.Id
                    }).ToArray(),
                SignsClientIds = tradeRecords
                    .Select(t => t.ClientId).Distinct().ToArray()
            };
        }

        private async Task<bool> ProcessOffchainMessage(IOffchainOrder offchainOrder, TradeQueueItem queueMessage)
        {
            var clientTransfer = (await _offchainTransferRepository.GetTransfersByOrder(offchainOrder.ClientId, offchainOrder.OrderId)).FirstOrDefault();
            var ethereumTxRequest = await _ethereumTransactionRequestRepository.GetByOrderAsync(offchainOrder.OrderId);

            var walletCredsMarket = await _walletCredentialsRepository.GetAsync(queueMessage.Trades[0].MarketClientId);
            var walletCredsLimit = await _walletCredentialsRepository.GetAsync(queueMessage.Trades[0].LimitClientId);

            var clientTrades = queueMessage.ToDomainOffchain(walletCredsMarket, walletCredsLimit, await _assetsService.GetAllAssetsAsync());

            var operations = AggregateSwaps(queueMessage.Trades);

            await CreateTransaction(offchainOrder.Id, operations, clientTrades);

            var notify = new HashSet<string>();
            try
            {
                if (ethereumTxRequest != null)
                {
                    var wasTransferOk = await ProcessEthGuaranteeTransfer(ethereumTxRequest, operations, clientTrades);

                    if (!wasTransferOk)
                        return true;
                }

                var sellOperations = operations.Where(x => x.Amount < 0);
                var buyOperations = operations.Where(x => x.Amount > 0);

                foreach (var operation in sellOperations)
                {
                    if (await _offchainIgnoreRepository.IsIgnored(operation.ClientId))
                    {
                        await _log.WriteInfoAsync(nameof(TradeQueue), nameof(ProcessOffchainMessage), $"Order: [{offchainOrder.OrderId}], data: [{operation.ToJson()}]", "Transfer ignored");
                        continue;
                    }

                    var asset = await _assetsService.TryGetAssetAsync(operation.AssetId);
                    if (asset.Blockchain == Blockchain.Ethereum)
                        continue;   //guarantee transfer already sent for eth


                    if (clientTransfer != null)
                    {
                        await _offchainTransferRepository.CompleteTransfer(clientTransfer.Id);

                        var change = clientTransfer.Amount - Math.Abs(operation.Amount);

                        if (change < 0)
                            await _log.WriteWarningAsync(nameof(TradeQueue), nameof(ProcessOffchainMessage),
                                $"Order: [{offchainOrder.OrderId}], data: [{operation.ToJson()}]",
                                "Diff is less than ZERO !");

                        if (change > 0)
                        {
                            await _offchainRequestService.CreateOffchainRequest(operation.TransferId, operation.ClientId,
                                operation.AssetId, change, offchainOrder.OrderId, OffchainTransferType.FromHub);
                            notify.Add(operation.ClientId);
                        }
                    }
                }

                foreach (var operation in buyOperations)
                {
                    if (await _offchainIgnoreRepository.IsIgnored(operation.ClientId))
                    {
                        await _log.WriteInfoAsync(nameof(TradeQueue), nameof(ProcessOffchainMessage), $"Order: [{offchainOrder.OrderId}], data: [{operation.ToJson()}]", "Transfer ignored");
                        continue;
                    }

                    var asset = await _assetsService.TryGetAssetAsync(operation.AssetId);
                    if (asset.Blockchain == Blockchain.Ethereum)
                    {
                        await ProcessEthBuy(operation, asset, clientTrades, offchainOrder.OrderId);
                        continue;
                    }

                    await _offchainRequestService.CreateOffchainRequest(operation.TransferId, operation.ClientId,
                        operation.AssetId, operation.Amount, offchainOrder.OrderId, OffchainTransferType.FromHub);
                    notify.Add(operation.ClientId);
                }
            }
            finally
            {
                await _clientTradesRepository.SaveAsync(clientTrades);

                foreach (var item in notify)
                    await _offchainRequestService.NotifyUser(item);
            }

            return true;
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
                var minAmountForAsset = (decimal) Math.Pow(10, -asset.Accuracy);
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

        private async Task CreateTransaction(string orderId, List<AggregatedTransfer> operations, IClientTrade[] trades)
        {
            var contextData = new SwapOffchainContextData();

            foreach (var operation in operations)
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

            await _bitcoinTransactionsRepository.CreateAsync(orderId, BitCoinCommands.SwapOffchain, "", contextData.ToJson(), "");
        }

        private List<AggregatedTransfer> AggregateSwaps(IEnumerable<TradeQueueItem.TradeInfo> swaps)
        {
            var list = new List<AggregatedTransfer>();

            foreach (var swap in swaps)
            {
                var amount1 = Convert.ToDecimal(swap.MarketVolume);
                var amount2 = Convert.ToDecimal(swap.LimitVolume);

                AddAmount(list, swap.MarketClientId, swap.MarketAsset, -amount1);
                AddAmount(list, swap.LimitClientId, swap.MarketAsset, amount1);

                AddAmount(list, swap.LimitClientId, swap.LimitAsset, -amount2);
                AddAmount(list, swap.MarketClientId, swap.LimitAsset, amount2);
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