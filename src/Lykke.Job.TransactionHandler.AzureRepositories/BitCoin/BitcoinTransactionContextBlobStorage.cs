﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;

namespace Lykke.Job.TransactionHandler.AzureRepositories.BitCoin
{
    public class BitcoinTransactionContextBlobStorage : IBitcoinTransactionContextBlobStorage
    {
        private const string BlobContainer = "bitcoin-transaction-context";

        private readonly IBlobStorage _storage;

        public BitcoinTransactionContextBlobStorage(IBlobStorage storage)
        {
            _storage = storage;
        }

        public async Task<string> Get(string transactionId)
        {
            if (await _storage.HasBlobAsync(BlobContainer, GetKey(transactionId)))
                return await _storage.GetAsTextAsync(BlobContainer, GetKey(transactionId));
            return null;
        }

        public async Task Set(string transactionId, string context)
        {
            await _storage.SaveBlobAsync(BlobContainer, GetKey(transactionId), Encoding.UTF8.GetBytes(context));
        }

        private string GetKey(string transactionId)
        {
            return $"{transactionId}.txt";
        }
    }
}
