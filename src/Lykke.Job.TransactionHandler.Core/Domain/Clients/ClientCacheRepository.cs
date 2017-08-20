using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Clients
{
    public interface IClientCache
    {
        int LimitOrdersCount { get; }
    }

    public interface IClientCacheRepository
    {
        Task<IClientCache> GetCache(string clientId);

        Task UpdateLimitOrdersCount(string clientId, int count);
    }
}
