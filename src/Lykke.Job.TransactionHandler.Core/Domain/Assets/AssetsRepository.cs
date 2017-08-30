using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Assets
{
    public interface IAsset
    {
        string Id { get; }
        string Name { get; }
        string Symbol { get; }
    }

    public interface IAssetsRepository
    {
        Task<IAsset> GetAssetAsync(string id);
    }
}
