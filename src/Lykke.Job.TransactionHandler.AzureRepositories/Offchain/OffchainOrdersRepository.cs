﻿using System;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.TransactionHandler.Core.Domain.Offchain;

namespace Lykke.Job.TransactionHandler.AzureRepositories.Offchain
{
    public class OffchainOrder : BaseEntity, IOffchainOrder
    {
        public string Id => RowKey;

        public string OrderId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Volume { get; set; }
        public decimal ReservedVolume { get; set; }
        public string AssetPair { get; set; }
        public string Asset { get; set; }
        public bool Straight { get; set; }
        public decimal Price { get; set; }
        public bool IsLimit { get; set; }


        public static string GeneratePartitionKey()
        {
            return "Order";
        }

        public static OffchainOrder Create(string clientId, string asset, string assetPair, decimal volume, decimal reservedVolme,
            bool straight, decimal price = 0)
        {
            var id = Guid.NewGuid().ToString();
            return new OffchainOrder
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = id,
                OrderId = id,
                ClientId = clientId,
                CreatedAt = DateTime.UtcNow,
                Volume = volume,
                ReservedVolume = reservedVolme,
                AssetPair = assetPair,
                Asset = asset,
                Straight = straight,
                Price = price,
                IsLimit = price > 0
            };
        }
    }

    public class OffchainOrderRepository : IOffchainOrdersRepository
    {
        private readonly INoSQLTableStorage<OffchainOrder> _storage;

        public OffchainOrderRepository(INoSQLTableStorage<OffchainOrder> storage)
        {
            _storage = storage;
        }

        public async Task<IOffchainOrder> GetOrder(string id)
        {
            return await _storage.GetDataAsync(OffchainOrder.GeneratePartitionKey(), id);
        }
    }
}