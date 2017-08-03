using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Services.Quanta
{
    public interface IQuantaService
    {
        Task SendCashOutRequest(string id, string addressTo, double amount);
    }
}