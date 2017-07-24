using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Quanta
{
    public interface IQuantaCommandProducer
    {
        Task ProduceCashOutCommand(string id, string addressTo, double amount);
    }
}