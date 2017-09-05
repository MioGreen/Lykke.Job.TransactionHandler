using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Offchain
{
    public interface IOffchainIgnore
    {
        string ClientId { get; }
    }

    public interface IOffchainIgnoreRepository
    {
        Task<IEnumerable<IOffchainIgnore>> GetIgnoredClients();
        Task<bool> IsIgnored(string client);
    }
}