using System;
using System.Threading.Tasks;
using Common.Log;
using LkeServices.Generated.QuantaApi;
using LkeServices.Generated.QuantaApi.Models;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;
using Lykke.Job.TransactionHandler.Core.Domain.Quanta;
using Lykke.Job.TransactionHandler.Core.Services.Quanta;
using Lykke.Job.TransactionHandler.Services.Notifications;

namespace Lykke.Job.TransactionHandler.Services.Quanta
{
    public class QuantaServiceSettings
    {
        public Uri BaseUri { get; set; }
    }

    public class QuantaService : IQuantaService
    {
        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly QuantaServiceSettings _settings;
        private readonly ILog _log;
        private readonly SrvSlackNotifications _srvSlackNotifications;
        private readonly IQuantaCommandProducer _quantaCommandProducer;

        public QuantaService(IWalletCredentialsRepository walletCredentialsRepository,
            QuantaServiceSettings settings, ILog log, SrvSlackNotifications srvSlackNotifications,
            IQuantaCommandProducer quantaCommandProducer)
        {
            _walletCredentialsRepository = walletCredentialsRepository;
            _settings = settings;
            _log = log;
            _srvSlackNotifications = srvSlackNotifications;
            _quantaCommandProducer = quantaCommandProducer;
        }

        private QuantaApiClient Api => new QuantaApiClient(_settings.BaseUri);

        public async Task<string> SetNewQuantaContract(IWalletCredentials walletCredentials)
        {
            try
            {
                var contract = (await Api.ApiClientRegisterGetAsync()) as RegisterResponse;

                if (contract != null)
                    await _walletCredentialsRepository.SetQuantaContract(walletCredentials.ClientId, contract.Contract);

                return contract?.Contract;
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(QuantaService), nameof(SetNewQuantaContract), "", ex);

                var msg = $"Quanta contract was not set for {walletCredentials.ClientId}.\n{ex.Message}";
                await _srvSlackNotifications.SendNotification(ChannelTypes.Errors, msg, "lykkeapi");
            }

            return null;
        }

        public Task SendCashOutRequest(string id, string addressTo, double amount)
        {
            return _quantaCommandProducer.ProduceCashOutCommand(id, addressTo, amount);
        }

        public async Task<bool> IsQuantaUser(string address)
        {
            var isQuantaUser = (await Api.ApiClientIsQuantaUserGetAsync(address)) as IsQuantaUserResponse;

            return isQuantaUser?.IsQuantaUser ?? false;
        }
    }
}