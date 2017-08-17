using System;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Offchain
{
    public interface IOffchainOrder
    {
        string Id { get; }
        string OrderId { get; }
        string ClientId { get; set; }
        DateTime CreatedAt { get; set; }
        decimal Volume { get; set; }
        string AssetPair { get; set; }
        string Asset { get; set; }
        bool Straight { get; set; }
        decimal Price { get; set; }
        bool IsLimit { get; set; }
    }

    public interface IOffchainOrdersRepository
    {
        Task<IOffchainOrder> GetOrder(string id);
        Task<IOffchainOrder> CreateOrder(string clientId, string asset, string assetPair, decimal volume, bool straight);
        Task UpdatePrice(string orderId, decimal price);
    }
}