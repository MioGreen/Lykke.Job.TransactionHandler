using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Services.BitCoin
{
    public interface IBitcoinTransactionService
    {
        Task<T> GetTransactionContext<T>(string transactionId);

        Task SetTransactionContext<T>(string transactionId, T context);

        Task CreateOrUpdateAsync(string meOrderId);
    }
}
