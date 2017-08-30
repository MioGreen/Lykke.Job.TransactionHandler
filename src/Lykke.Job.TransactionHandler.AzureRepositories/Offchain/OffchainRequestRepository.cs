﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.TransactionHandler.Core.Domain.Offchain;

namespace Lykke.Job.TransactionHandler.AzureRepositories.Offchain
{
    public class OffchainRequestEntity : BaseEntity, IOffchainRequest
    {
        public string RequestId => RowKey;
        public string TransferId { get; set; }

        public string AssetId { get; set; }

        public string ClientId { get; set; }

        public RequestType Type { get; set; }

        public DateTime? StartProcessing { get; set; }

        public DateTime CreateDt { get; set; }

        public int TryCount { get; set; }

        public OffchainTransferType TransferType { get; set; }

        public DateTime? ServerLock { get; set; }

        public static class ByRecord
        {
            public static string Partition = "OffchainSignatureRequestEntity";

            public static OffchainRequestEntity Create(string id, string transferId, string clientId, string assetId, RequestType type, OffchainTransferType transferType, DateTime? serverLock)
            {
                var item = CreateNew(transferId, clientId, assetId, type, transferType, serverLock);

                item.PartitionKey = Partition;
                item.RowKey = id;

                return item;
            }
        }

        public static class ByClient
        {
            public static string GeneratePartition(string clientId)
            {
                return clientId;
            }

            public static OffchainRequestEntity Create(string id, string transferId, string clientId, string assetId, RequestType type, OffchainTransferType transferType, DateTime? serverLock)
            {
                var item = CreateNew(transferId, clientId, assetId, type, transferType, serverLock);

                item.PartitionKey = GeneratePartition(clientId);
                item.RowKey = id;

                return item;
            }
        }

        public static class Archieved
        {
            public static string GeneratePartition()
            {
                return "Archieved";
            }

            public static OffchainRequestEntity Create(IOffchainRequest request)
            {
                return new OffchainRequestEntity
                {
                    RowKey = request.RequestId,
                    PartitionKey = GeneratePartition(),
                    TransferId = request.TransferId,
                    ClientId = request.ClientId,
                    AssetId = request.AssetId,
                    Type = request.Type,
                    CreateDt = request.CreateDt == DateTime.MinValue ? DateTime.UtcNow : request.CreateDt,
                    TryCount = request.TryCount,
                    TransferType = request.TransferType,
                    ServerLock = request.ServerLock,
                    StartProcessing = request.StartProcessing
                };
            }
        }

        public static OffchainRequestEntity CreateNew(string transferId, string clientId, string assetId, RequestType type, OffchainTransferType transferType, DateTime? serverLock = null)
        {
            return new OffchainRequestEntity
            {
                TransferId = transferId,
                ClientId = clientId,
                AssetId = assetId,
                Type = type,
                CreateDt = DateTime.UtcNow,
                TransferType = transferType,
                ServerLock = serverLock
            };
        }
    }


    public class OffchainRequestRepository : IOffchainRequestRepository
    {
        private const int LockTimeoutSeconds = 100;

        private readonly INoSQLTableStorage<OffchainRequestEntity> _table;

        public OffchainRequestRepository(INoSQLTableStorage<OffchainRequestEntity> table)
        {
            _table = table;
        }

        public async Task<IOffchainRequest> CreateRequest(string transferId, string clientId, string assetId, RequestType type, OffchainTransferType transferType, DateTime? serverLock = null)
        {
            var id = Guid.NewGuid().ToString();

            var byClient = OffchainRequestEntity.ByClient.Create(id, transferId, clientId, assetId, type, transferType, serverLock);
            await _table.InsertAsync(byClient);

            var byRecord = OffchainRequestEntity.ByRecord.Create(id, transferId, clientId, assetId, type, transferType, serverLock);
            await _table.InsertAsync(byRecord);

            return byRecord;
        }

        public async Task<IEnumerable<IOffchainRequest>> GetRequestsForClient(string clientId)
        {
            return await _table.GetDataAsync(OffchainRequestEntity.ByClient.GeneratePartition(clientId));
        }

        public async Task<IEnumerable<IOffchainRequest>> GetCurrentRequests()
        {
            return await _table.GetDataAsync(OffchainRequestEntity.ByRecord.Partition);
        }

        public async Task<IOffchainRequest> GetRequest(string requestId)
        {
            return await _table.GetDataAsync(OffchainRequestEntity.ByRecord.Partition, requestId);
        }

        public async Task<IOffchainRequest> CreateRequestAndLock(string transferId, string clientId, string assetId,
            RequestType type, OffchainTransferType transferType, DateTime? lockDate)
        {
            var existingRequest = (await GetRequestsForClient(clientId)).FirstOrDefault(x => x.AssetId == assetId
                                                                                        && x.TransferType == transferType
                                                                                        && x.StartProcessing == null);

            if (existingRequest == null)
                return await CreateRequest(transferId, clientId, assetId, type, transferType, lockDate);

            var replaced = await _table.MergeAsync(OffchainRequestEntity.ByRecord.Partition, existingRequest.RequestId, entity =>
            {
                if (entity.StartProcessing != null)
                    return null;

                entity.ServerLock = lockDate;
                return entity;
            });

            if (replaced == null)
                return await CreateRequest(transferId, clientId, assetId, type, transferType, lockDate);

            return replaced;
        }

        public async Task<IOffchainRequest> LockRequest(string requestId)
        {
            return await _table.ReplaceAsync(OffchainRequestEntity.ByRecord.Partition, requestId, entity =>
            {
                if (entity.StartProcessing == null || (DateTime.UtcNow - entity.StartProcessing.Value).TotalSeconds > LockTimeoutSeconds)
                {
                    entity.StartProcessing = DateTime.UtcNow;
                    entity.TryCount++;

                    //TODO: remove
                    if (entity.CreateDt == DateTime.MinValue)
                        entity.CreateDt = DateTime.UtcNow;

                    return entity;
                }
                return null;
            });
        }

        public async Task Complete(string requestId)
        {
            var record = await _table.DeleteAsync(OffchainRequestEntity.ByRecord.Partition, requestId);

            await _table.DeleteAsync(OffchainRequestEntity.ByClient.GeneratePartition(record.ClientId), requestId);

            await _table.InsertOrReplaceAsync(OffchainRequestEntity.Archieved.Create(record));
        }
    }

}