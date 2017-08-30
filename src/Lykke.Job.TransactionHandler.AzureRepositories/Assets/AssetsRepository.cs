using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.TransactionHandler.Core.Domain.Assets;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.TransactionHandler.AzureRepositories.Assets
{
    public class AssetEntity : TableEntity, IAsset
    {
        public static string GeneratePartitionKey()
        {
            return "Asset";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public string Name { get; set; }
        public string Symbol { get; set; }
    }

    public class AssetsRepository : IAssetsRepository
    {
        private readonly INoSQLTableStorage<AssetEntity> _tableStorage;

        public AssetsRepository(INoSQLTableStorage<AssetEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IAsset> GetAssetAsync(string id)
        {
            var partitionKey = AssetEntity.GeneratePartitionKey();
            var rowKey = AssetEntity.GenerateRowKey(id);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }
    }
}
