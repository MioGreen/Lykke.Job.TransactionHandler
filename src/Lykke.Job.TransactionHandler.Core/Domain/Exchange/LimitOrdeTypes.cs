using System;
using System.Collections.Generic;

namespace Lykke.Job.TransactionHandler.Core.Domain.Exchange
{
    public class MatchedOrder
    {
        public string Id { get; set; }
        public double Volume { get; set; }

        internal static MatchedOrder Create(IOrderBase orderBase, double volume)
        {
            return new MatchedOrder
            {
                Id = orderBase.Id,
                Volume = volume
            };
        }
    }

    public interface ILimitOrder : IOrderBase
    {
        double RemainingVolume { get; set; }
    }

    public class LimitOrder : ILimitOrder
    {
        public DateTime CreatedAt { get; set; }
        public DateTime MatchedAt { get; set; }
        public string Id { get; set; }
        public List<MatchedOrder> MatchedOrders { get; set; }
        public string ClientId { get; set; }
        public string BaseAsset { get; set; }
        public string AssetPairId { get; set; }
        public string Status { get; set; }
        public bool Straight { get; set; }
        public OrderAction OrderAction { get; set; }
        public string BlockChain { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public double RemainingVolume { get; set; }

    }
}