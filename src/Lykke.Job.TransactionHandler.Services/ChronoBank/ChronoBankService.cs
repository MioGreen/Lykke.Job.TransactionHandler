using System.Threading.Tasks;
using Lykke.Job.TransactionHandler.Core.Domain.ChronoBank;
using Lykke.Job.TransactionHandler.Core.Services.ChronoBank;

namespace Lykke.Job.TransactionHandler.Services.ChronoBank
{
    public class ChronoBankService : IChronoBankService
    {
        private readonly IChronoBankCommandProducer _chronoBankCommandProducer;

        public ChronoBankService(IChronoBankCommandProducer chronoBankCommandProducer)
        {
            _chronoBankCommandProducer = chronoBankCommandProducer;
        }

        public Task SendCashOutRequest(string id, string addressTo, double amount)
        {
            return _chronoBankCommandProducer.ProduceCashOutCommand(id, addressTo, amount);
        }
    }
}