using System.Threading.Tasks;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;

namespace Lykke.Job.TransactionHandler.Core.Services.Quanta
{
    public interface IQuantaService
    {
        Task<string> SetNewQuantaContract(IWalletCredentials walletCredentials);
        Task SendCashOutRequest(string id, string addressTo, double amount);
        Task<bool> IsQuantaUser(string address);
    }
}