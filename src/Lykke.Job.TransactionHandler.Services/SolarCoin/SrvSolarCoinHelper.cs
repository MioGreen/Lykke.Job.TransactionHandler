using System.Threading.Tasks;
using Lykke.Job.TransactionHandler.Core.Domain.SolarCoin;
using Lykke.Job.TransactionHandler.Core.Services.SolarCoin;

namespace Lykke.Job.TransactionHandler.Services.SolarCoin
{
    public class SrvSolarCoinHelper : ISrvSolarCoinHelper
    {
        private readonly ISrvSolarCoinCommandProducer _solarCoinCommandProducer;

        public SrvSolarCoinHelper(ISrvSolarCoinCommandProducer solarCoinCommandProducer)
        {
            _solarCoinCommandProducer = solarCoinCommandProducer;
        }

        public Task SendCashOutRequest(string id, SolarCoinAddress addressTo, double amount)
        {
            return _solarCoinCommandProducer.ProduceCashOutCommand(id, addressTo, amount);
        }
    }

    #region Response Models

    public class GetAddressModel
    {
        public string Address { get; set; }
    }

    #endregion
}