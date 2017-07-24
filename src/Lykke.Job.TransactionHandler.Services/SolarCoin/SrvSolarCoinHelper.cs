using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.TransactionHandler.Core;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;
using Lykke.Job.TransactionHandler.Core.Domain.SolarCoin;
using Lykke.Job.TransactionHandler.Core.Services.SolarCoin;
using Lykke.Job.TransactionHandler.Services.Http;
using Lykke.Job.TransactionHandler.Services.Notifications;

namespace Lykke.Job.TransactionHandler.Services.SolarCoin
{
    public class SrvSolarCoinHelper : ISrvSolarCoinHelper
    {
        private readonly AppSettings.SolarCoinSettings _settings;
        private readonly ILog _log;
        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly SrvSlackNotifications _srvSlackNotifications;
        private readonly ISrvSolarCoinCommandProducer _solarCoinCommandProducer;

        public SrvSolarCoinHelper(AppSettings.SolarCoinSettings settings, ILog log,
            IWalletCredentialsRepository walletCredentialsRepository, SrvSlackNotifications srvSlackNotifications,
            ISrvSolarCoinCommandProducer solarCoinCommandProducer)
        {
            _settings = settings;
            _log = log;
            _walletCredentialsRepository = walletCredentialsRepository;
            _srvSlackNotifications = srvSlackNotifications;
            _solarCoinCommandProducer = solarCoinCommandProducer;
        }

        public async Task<string> SetNewSolarCoinAddress(IWalletCredentials walletCredentials)
        {
            try
            {
                var address =
                    (await new HttpRequestClient().GetRequest(_settings.GetAddressUrl))
                    .DeserializeJson<GetAddressModel>().Address;

                await _walletCredentialsRepository.SetSolarCoinWallet(walletCredentials.ClientId, address);

                return address;
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync("SolarCoin", "GetAddress", "", ex);

                var msg = $"SolarCoin address was not set for {walletCredentials.ClientId}.\n{ex.Message}";
                await _srvSlackNotifications.SendNotification(ChannelTypes.Errors, msg, "lykkeapi");
            }

            return null;
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