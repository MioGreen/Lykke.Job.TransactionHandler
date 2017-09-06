using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;
using Lykke.Job.TransactionHandler.Core.Services.BitCoin;

namespace Lykke.Job.TransactionHandler.Services.BitCoin
{
    public class BitcoinTransactionService : IBitcoinTransactionService
    {
        private readonly IBitCoinTransactionsRepository _bitCoinTransactionsRepository;
        private readonly IBitcoinTransactionContextBlobStorage _contextBlobStorage;

        public BitcoinTransactionService(IBitCoinTransactionsRepository bitCoinTransactionsRepository, IBitcoinTransactionContextBlobStorage contextBlobStorage)
        {
            _bitCoinTransactionsRepository = bitCoinTransactionsRepository;
            _contextBlobStorage = contextBlobStorage;
        }

        public async Task<T> GetTransactionContext<T>(string transactionId)
        {
            var fromBlob = await _contextBlobStorage.Get(transactionId);
            if (string.IsNullOrWhiteSpace(fromBlob))
            {
                var transaction = await _bitCoinTransactionsRepository.FindByTransactionIdAsync(transactionId);
                fromBlob = transaction?.ContextData;
            }

            if (fromBlob == null)
                return default(T);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(fromBlob);
        }

        public Task SetTransactionContext<T>(string transactionId, T context)
        {
            return _contextBlobStorage.Set(transactionId, context.ToJson());
        }

        public Task CreateOrUpdateAsync(string meOrderId)
        {
            return _bitCoinTransactionsRepository.CreateOrUpdateAsync(meOrderId, BitCoinCommands.SwapOffchain);
        }
    }
}
