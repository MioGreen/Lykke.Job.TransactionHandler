using System.Threading.Tasks;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;

namespace Lykke.Job.TransactionHandler.Core.Services.ChronoBank
{
    public interface IChronoBankService
    {
        Task<string> SetNewChronoBankContract(IWalletCredentials walletCredentials);
        Task SendCashOutRequest(string id, string addressTo, double amount);
    }
}