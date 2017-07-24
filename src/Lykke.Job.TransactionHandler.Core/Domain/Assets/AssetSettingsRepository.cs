using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Assets
{
    public interface IAssetSetting
    {
        string Asset { get; }
        string HotWallet { get; set; }
        string ChangeWallet { get; set; }
        decimal CashinCoef { get; set; }
        decimal Dust { get; set; }
        int MaxOutputsCountInTx { get; set; }
        decimal MinBalance { get; set; }
        decimal OutputSize { get; set; }
        int MinOutputsCount { get; set; }
        int MaxOutputsCount { get; set; }
        int PrivateIncrement { get; set; }
    }

    public class AssetSetting : IAssetSetting
    {
        public string Asset { get; set; }
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
    }

    public interface IAssetSettingRepository
    {
        Task<IEnumerable<IAssetSetting>> GetAssetSettings();

        Task InsertOrReplaceSetting(IAssetSetting setting);
        Task<IAssetSetting> GetAssetSetting(string id);
        Task DeleteAssetSetting(string id);
    }
}