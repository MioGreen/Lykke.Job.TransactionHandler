using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Services.TrustedWallet
{
    public interface ITrustedWalletService
    {
        Task Deposit(string walletId, string assetId, decimal amount);
    }
}
