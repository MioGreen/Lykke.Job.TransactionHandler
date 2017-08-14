using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;
using Lykke.Job.TransactionHandler.Core.Domain.CashOperations;
using Lykke.Job.TransactionHandler.Core.Domain.Exchange;
using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Queues.Models
{
    public class LimitQueueItem
    {
        [JsonProperty("orders")]
        public List<LimitOrderWithTrades> Orders { get; set; }

        public class LimitOrderWithTrades
        {
            [JsonProperty("order")]
            public LimitOrder Order { get; set; }

            [JsonProperty("trades")]
            public List<LimitTradeInfo> Trades { get; set; }
        }

        public class LimitOrder : ILimitOrder
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("externalId")]
            public string ExternalId { get; set; }

            [JsonProperty("assetPairId")]
            public string AssetPairId { get; set; }

            [JsonProperty("clientId")]
            public string ClientId { get; set; }

            [JsonProperty("volume")]
            public double Volume { get; set; }

            [JsonProperty("price")]
            public double Price { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("createdAt")]
            public DateTime CreatedAt { get; set; }

            [JsonProperty("registered")]
            public DateTime Registered { get; set; }

            [JsonProperty("remainingVolume")]
            public double RemainingVolume { get; set; }

            public bool Straight { get; set; }
        }

        public class LimitTradeInfo
        {
            [JsonProperty("clientId")]
            public string ClientId { get; set; }

            [JsonProperty("asset")]
            public string Asset { get; set; }

            [JsonProperty("volume")]
            public double Volume { get; set; }

            [JsonProperty("price")]
            public double Price { get; set; }

            [JsonProperty("timestamp")]
            public DateTime Timestamp { get; set; }

            [JsonProperty("oppositeOrderId")]
            public string OppositeOrderId { get; set; }

            [JsonProperty("oppositeOrderExternalId")]
            public string OppositeOrderExternalId { get; set; }

            [JsonProperty("oppositeAsset")]
            public string OppositeAsset { get; set; }

            [JsonProperty("oppositeClientId")]
            public string OppositeClientId { get; set; }

            [JsonProperty("oppositeVolume")]
            public double OppositeVolume { get; set; }
        }
    }

    public static class LimitExt
    {
        public static IClientTrade[] GetTradeRecords(this LimitQueueItem.LimitTradeInfo trade, ILimitOrder limitOrder,
            string btcTransactionId, IWalletCredentials walletCredentialsLimitA,
            IWalletCredentials walletCredentialsLimitB)
        {
            var result = new List<IClientTrade>();

            result.AddRange(CreateTradeRecordsForClientWithVolumes(trade, limitOrder, btcTransactionId, walletCredentialsLimitA, walletCredentialsLimitB, trade.Volume, trade.OppositeVolume));

            return result.ToArray();
        }

        private static IClientTrade[] CreateTradeRecordsForClientWithVolumes(LimitQueueItem.LimitTradeInfo trade,
            ILimitOrder limitOrder,
            string btcTransactionId, IWalletCredentials walletCredentialsLimitA,
            IWalletCredentials walletCredentialsLimitB, double marketVolume, double limitVolume)
        {
            var clientId = walletCredentialsLimitA.ClientId;

            var mutlisig = walletCredentialsLimitA.MultiSig;
            var fromMultisig = walletCredentialsLimitB.MultiSig;

            var marketAssetRecord = CreateCommonPartForTradeRecord(trade, limitOrder, btcTransactionId);
            var limitAssetRecord = CreateCommonPartForTradeRecord(trade, limitOrder, btcTransactionId);

            marketAssetRecord.ClientId = limitAssetRecord.ClientId = clientId;
            marketAssetRecord.AddressFrom = limitAssetRecord.AddressFrom = fromMultisig;
            marketAssetRecord.AddressTo = limitAssetRecord.AddressTo = mutlisig;
            marketAssetRecord.Multisig = limitAssetRecord.Multisig = mutlisig;

            marketAssetRecord.Amount = marketVolume * -1;
            marketAssetRecord.AssetId = trade.Asset;

            limitAssetRecord.Amount = limitVolume;
            limitAssetRecord.AssetId = trade.OppositeAsset;

            marketAssetRecord.Id = Utils.GenerateRecordId(marketAssetRecord.DateTime);
            limitAssetRecord.Id = Utils.GenerateRecordId(limitAssetRecord.DateTime);

            return new IClientTrade[] {marketAssetRecord, limitAssetRecord};
        }

        private static ClientTrade CreateCommonPartForTradeRecord(LimitQueueItem.LimitTradeInfo trade, ILimitOrder limitOrder,
            string btcTransactionId)
        {
            return new ClientTrade
            {
                DateTime = trade.Timestamp,
                Price = trade.Price,
                LimitOrderId = limitOrder.ExternalId,
                OppositeLimitOrderId = trade.OppositeOrderId,
                TransactionId = btcTransactionId,
                IsLimitOrderResult = true
            };
        }
    }
}
