﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;

namespace Lykke.Job.TransactionHandler.Core.Services.BitCoin
{
    public interface IBitcoinTransactionService
    {
        Task<T> GetTransactionContext<T>(string transactionId) where T : BaseContextData;

        Task SetTransactionContext<T>(string transactionId, T context) where T : BaseContextData;

        Task SetStringTransactionContext(string transactionId, string context);

        Task CreateOrUpdateAsync(string meOrderId);
    }
}
