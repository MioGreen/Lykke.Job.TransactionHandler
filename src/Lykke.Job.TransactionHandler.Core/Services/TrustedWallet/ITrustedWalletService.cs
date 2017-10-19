using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Services.TrustedWallet
{
    public interface ITrustedWalletService
    {
        Task SendCashInRequest(string clientId, string walletId, string assetId, decimal amount);
    }
}
