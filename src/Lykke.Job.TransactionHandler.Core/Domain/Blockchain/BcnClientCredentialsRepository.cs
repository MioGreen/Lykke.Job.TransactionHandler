using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Blockchain
{
    public interface IBcnCredentialsRecord
    {
        string Address { get; set; }
        string EncodedKey { get; set; }
        string PublicKey { get; set; }
        string AssetId { get; set; }
        string ClientId { get; set; }
        string AssetAddress { get; set; }
    }

    public class BcnCredentialsRecord : IBcnCredentialsRecord
    {
        public string Address { get; set; }
        public string EncodedKey { get; set; }
        public string PublicKey { get; set; }
        public string ClientId { get; set; }
        public string AssetAddress { get; set; }
        public string AssetId { get; set; }
    }

    public interface IBcnClientCredentialsRepository
    {
        Task SaveAsync(IBcnCredentialsRecord credsRecord);
        Task<IBcnCredentialsRecord> GetAsync(string clientId, string assetId);
        Task<IBcnCredentialsRecord> GetByAssetAddressAsync(string assetAddress);
        Task<IEnumerable<IBcnCredentialsRecord>> GetAsync(string clientId);
        Task<string> GetClientAddress(string clientId);
    }

    public static class Ext
    {
        public static string GetAssetAddress(this IEnumerable<IBcnCredentialsRecord> creds, string assetId)
        {
            return creds.FirstOrDefault(x => x.AssetId == assetId)?.AssetAddress;
        }
    }

}