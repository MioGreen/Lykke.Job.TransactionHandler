using System.Threading.Tasks;
using Lykke.Job.TransactionHandler.Core.Domain.Quanta;
using Lykke.Job.TransactionHandler.Core.Services.Quanta;

namespace Lykke.Job.TransactionHandler.Services.Quanta
{
    public class QuantaService : IQuantaService
    {
        private readonly IQuantaCommandProducer _quantaCommandProducer;

        public QuantaService(IQuantaCommandProducer quantaCommandProducer)
        {
            _quantaCommandProducer = quantaCommandProducer;
        }

        public Task SendCashOutRequest(string id, string addressTo, double amount)
        {
            return _quantaCommandProducer.ProduceCashOutCommand(id, addressTo, amount);
        }
    }
}