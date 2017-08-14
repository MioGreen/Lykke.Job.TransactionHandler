using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.TransactionHandler.Core.Domain.Exchange;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.TransactionHandler.AzureRepositories.Exchange
{
    public class LimitOrderEntity : BaseEntity, ILimitOrder
    {

        public static class ByOrderId
        {
            public static string GeneratePartitionKey()
            {
                return "OrderId";
            }

            public static string GenerateRowKey(string orderId)
            {
                return orderId;
            }

            public static LimitOrderEntity Create(ILimitOrder limitOrder)
            {
                var entity = CreateNew(limitOrder);
                entity.RowKey = GenerateRowKey(limitOrder.ExternalId);
                entity.PartitionKey = GeneratePartitionKey();
                return entity;
            }
        }

        public static class ByClientId
        {
            public static string GeneratePartitionKey(string clientId)
            {
                return clientId;
            }

            public static string GenerateRowKey(string orderId)
            {
                return orderId;
            }

            public static LimitOrderEntity Create(ILimitOrder limitOrder)
            {
                var entity = CreateNew(limitOrder);
                entity.RowKey = GenerateRowKey(limitOrder.ExternalId);
                entity.PartitionKey = GeneratePartitionKey(limitOrder.ClientId);
                return entity;
            }
        }

        public static LimitOrderEntity CreateNew(ILimitOrder limitOrder)
        {
            return new LimitOrderEntity
            {
                AssetPairId = limitOrder.AssetPairId,
                ClientId = limitOrder.ClientId,
                CreatedAt = limitOrder.CreatedAt,
                Id = limitOrder.Id,
                Price = limitOrder.Price,
                Status = limitOrder.Status,
                Straight = limitOrder.Straight,
                Volume = limitOrder.Volume,
                RemainingVolume = limitOrder.RemainingVolume,
                ExternalId = limitOrder.ExternalId
            };
        }

        public DateTime CreatedAt { get; set; }

        public double Price { get; set; }
        public string AssetPairId { get; set; }

        public double Volume { get; set; }

        public string Status { get; set; }
        public bool Straight { get; set; }
        public string Id { get; set; }
        public string ClientId { get; set; }

        public double RemainingVolume { get; set; }
        public string ExternalId { get; set; }
    }

    public class LimitOrdersRepository : ILimitOrdersRepository
    {
        private readonly INoSQLTableStorage<LimitOrderEntity> _tableStorage;

        public LimitOrdersRepository(INoSQLTableStorage<LimitOrderEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task CreateOrUpdateAsync(ILimitOrder limitOrder)
        {
            var status = (OrderStatus)Enum.Parse(typeof(OrderStatus), limitOrder.Status);

            var byOrderEntity = LimitOrderEntity.ByOrderId.Create(limitOrder);
            var byClientEntity = LimitOrderEntity.ByClientId.Create(limitOrder);

            await _tableStorage.InsertOrMergeAsync(byOrderEntity);

            if (status == OrderStatus.InOrderBook || status == OrderStatus.Processing)
                await _tableStorage.InsertOrMergeAsync(byClientEntity);
            else
                await _tableStorage.DeleteAsync(LimitOrderEntity.ByClientId.GeneratePartitionKey(limitOrder.ClientId), limitOrder.ExternalId);
        }
    }
}