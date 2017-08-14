using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Exchange
{
    public interface ILimitOrder : IOrderBase
    {
        double RemainingVolume { get; set; }
        string ExternalId { get; set; }
    }

    public class LimitOrder : ILimitOrder
    {
        public DateTime CreatedAt { get; set; }
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string BaseAsset { get; set; }
        public string AssetPairId { get; set; }
        public string Status { get; set; }
        public bool Straight { get; set; }
        public OrderType OrderType { get; set; }
        public string BlockChain { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public double RemainingVolume { get; set; }
        public string ExternalId { get; set; }
    }

    public interface ILimitOrdersRepository
    {
        Task CreateOrUpdateAsync(ILimitOrder marketOrder);
    }
}