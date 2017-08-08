﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Common.PasswordTools;
using Lykke.Job.TransactionHandler.Core.Domain.Clients;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.TransactionHandler.AzureRepositories.Clients
{
    public class ClientPartnerRelationEntity : TableEntity
    {
        public static string GeneratePartitionKey(string email)
        {
            return $"TraderPartnerRelation_{email}";
        }

        public static string GenerateRowKey(string partnerId)
        {
            return partnerId;
        }

        public DateTime Registered { get; set; }
        public string Id => RowKey;
        public string Email { get; set; }
        public string PartnerId { get; set; }
        public string ClientId { get; set; }

        public static ClientPartnerRelationEntity CreateNew(string email, string clientId, string partnerId)
        {
            string partnerPublicId = partnerId ?? "";
            string clientEmail = email.ToLower();
            var result = new ClientPartnerRelationEntity
            {
                PartitionKey = GeneratePartitionKey(clientEmail),
                RowKey = GenerateRowKey(partnerPublicId),
                Email = clientEmail,
                PartnerId = partnerPublicId,
                ClientId = clientId,
                Registered = DateTime.UtcNow
            };

            return result;
        }
    }

    public static class ClientPhoneIndexHelper
    {
        private const string _phonePrefix = "ClientPhone_";
        public static string GeneratePhonePartitionKey(string phoneNumber) => $"{_phonePrefix}{phoneNumber}";
        public static string GeneratePhoneRowKey(string clientId) => $"{_phonePrefix}{clientId}";

        public static string ExtractClientId(string rowKey)
        {
            if (!rowKey.Contains(_phonePrefix))
                return null;

            return rowKey.Substring(_phonePrefix.Length);
        }
    }

    public class ClientAccountEntity : TableEntity, IClientAccount, IPasswordKeeping
    {
        public static string GeneratePartitionKey()
        {
            return "Trader";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public static IEqualityComparer<ClientAccountEntity> ComparerById { get; } = new EqualityComparerById();

        public DateTime Registered { get; set; }
        public string Id => RowKey;
        public string Email { get; set; }        
        public string Pin { get; set; }
        public string NotificationsId { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public string PartnerId { get; set; }

        public bool IsReviewAccount { get; set; }

        private class EqualityComparerById : IEqualityComparer<ClientAccountEntity>
        {
            public bool Equals(ClientAccountEntity x, ClientAccountEntity y)
            {
                if (x == y)
                    return true;
                if (x == null || y == null)
                    return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(ClientAccountEntity obj)
            {
                if (obj?.Id == null)
                    return 0;
                return obj.Id.GetHashCode();
            }
        }
    }


    public class ClientsRepository : IClientAccountsRepository
    {
        private readonly INoSQLTableStorage<ClientAccountEntity> _clientsTablestorage;

        public ClientsRepository(INoSQLTableStorage<ClientAccountEntity> clientsTablestorage)
        {
            _clientsTablestorage = clientsTablestorage;
        }

        public async Task<IClientAccount> GetByIdAsync(string id)
        {
            var partitionKey = ClientAccountEntity.GeneratePartitionKey();
            var rowKey = ClientAccountEntity.GenerateRowKey(id);

            return await _clientsTablestorage.GetDataAsync(partitionKey, rowKey);
        }
    }

}