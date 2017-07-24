using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Exchange
{
    public class MatchedLimitOrder : MatchedOrder
    {
        public double Price { get; set; }

        public static MatchedLimitOrder Create(ILimitOrder limitOrder, double volume)
        {
            return new MatchedLimitOrder
            {
                Price = limitOrder.Price,
                Id = limitOrder.Id,
                Volume = volume
            };
        }
    }

    public interface IMarketOrder : IOrderBase
    {
        DateTime MatchedAt { get; }
    }

    public class MarketOrder : IMarketOrder
    {
        public DateTime CreatedAt { get; set; }
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string AssetPairId { get; set; }
        public OrderAction OrderAction { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public bool Straight { get; set; }

        public DateTime MatchedAt { get; set; }
    }

    public interface IMarketOrdersRepository
    {
        Task CreateAsync(IMarketOrder marketOrder);
        Task<IMarketOrder> GetAsync(string orderId);
        Task<IMarketOrder> GetAsync(string clientId, string orderId);
        Task<IEnumerable<IMarketOrder>> GetOrdersAsync(string clientId);
        Task<IEnumerable<IMarketOrder>> GetOrdersAsync(IEnumerable<string> orderIds);
    }
}