using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Services.ChronoBank
{
    public interface IChronoBankService
    {
        Task SendCashOutRequest(string id, string addressTo, double amount);
    }
}