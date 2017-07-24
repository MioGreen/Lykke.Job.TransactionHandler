using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.TransactionHandler.Core.Domain.Offchain;

namespace Lykke.Job.TransactionHandler.AzureRepositories.Offchain
{
    public class OffchainIgnoreEntity : BaseEntity, IOffchainIgnore
    {
        public string ClientId => RowKey;

        public static string GeneratePartitionKey()
        {
            return "Data";
        }
    }

    public class OffchainIgnoreRepository : IOffchainIgnoreRepository
    {
        private readonly INoSQLTableStorage<OffchainIgnoreEntity> _storage;

        public OffchainIgnoreRepository(INoSQLTableStorage<OffchainIgnoreEntity> storage)
        {
            _storage = storage;
        }

        public async Task<bool> IsIgnored(string client)
        {
            var entity = await _storage.GetDataAsync(OffchainIgnoreEntity.GeneratePartitionKey(), client);
            return entity != null;
        }
    }
}