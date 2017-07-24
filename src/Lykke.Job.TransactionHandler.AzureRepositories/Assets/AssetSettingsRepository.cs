using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.TransactionHandler.Core.Domain.Assets;

namespace Lykke.Job.TransactionHandler.AzureRepositories.Assets
{
    public class AssetSettingEntity : BaseEntity, IAssetSetting
    {
        public static string GeneratePartitionKey()
        {
            return "Asset";
        }

        public string Asset => RowKey;
        public string HotWallet { get; set; }
        public string ChangeWallet { get; set; }
        public decimal CashinCoef { get; set; }
        public decimal Dust { get; set; }
        public int MaxOutputsCountInTx { get; set; }
        public decimal MinBalance { get; set; }
        public decimal OutputSize { get; set; }
        public int MinOutputsCount { get; set; }
        public int MaxOutputsCount { get; set; }
        public int PrivateIncrement { get; set; }

        public static AssetSettingEntity Create(IAssetSetting src)
        {
            return new AssetSettingEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = src.Asset,
                HotWallet = src.HotWallet,
                ChangeWallet = src.ChangeWallet,
                CashinCoef = src.CashinCoef,
                Dust = src.Dust,
                MaxOutputsCountInTx = src.MaxOutputsCountInTx,
                MinOutputsCount = src.MinOutputsCount,
                MaxOutputsCount = src.MaxOutputsCount,
                MinBalance = src.MinBalance,
                OutputSize = src.OutputSize,
                PrivateIncrement = src.PrivateIncrement
            };
        }
    }



    public class AssetSettingRepository : IAssetSettingRepository
    {
        private readonly INoSQLTableStorage<AssetSettingEntity> _table;

        public AssetSettingRepository(INoSQLTableStorage<AssetSettingEntity> table)
        {
            _table = table;
        }

        public async Task<IEnumerable<IAssetSetting>> GetAssetSettings()
        {
            return await _table.GetDataAsync(AssetSettingEntity.GeneratePartitionKey());
        }

        public Task InsertOrReplaceSetting(IAssetSetting setting)
        {
            return _table.InsertOrReplaceAsync(AssetSettingEntity.Create(setting));
        }

        public async Task<IAssetSetting> GetAssetSetting(string id)
        {
            return await _table.GetDataAsync(AssetSettingEntity.GeneratePartitionKey(), id);
        }

        public Task DeleteAssetSetting(string id)
        {
            return _table.DeleteAsync(AssetSettingEntity.GeneratePartitionKey(), id);
        }
    }
}