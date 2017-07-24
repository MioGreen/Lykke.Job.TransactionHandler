using System;
using System.Threading.Tasks;
using Common.Log;
using LkeServices.Generated.ChronoBankApi;
using LkeServices.Generated.ChronoBankApi.Models;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;
using Lykke.Job.TransactionHandler.Core.Domain.ChronoBank;
using Lykke.Job.TransactionHandler.Core.Services.ChronoBank;
using Lykke.Job.TransactionHandler.Services.Notifications;

namespace Lykke.Job.TransactionHandler.Services.ChronoBank
{
    public class ChronoBankServiceSettings
    {
        public Uri BaseUri { get; set; }
    }

    public class ChronoBankService : IChronoBankService
    {
        private readonly IWalletCredentialsRepository _walletCredentialsRepository;
        private readonly ChronoBankServiceSettings _settings;
        private readonly ILog _log;
        private readonly SrvSlackNotifications _srvSlackNotifications;
        private readonly IChronoBankCommandProducer _chronoBankCommandProducer;

        public ChronoBankService(IWalletCredentialsRepository walletCredentialsRepository,
            ChronoBankServiceSettings settings, ILog log, SrvSlackNotifications srvSlackNotifications,
            IChronoBankCommandProducer chronoBankCommandProducer)
        {
            _walletCredentialsRepository = walletCredentialsRepository;
            _settings = settings;
            _log = log;
            _srvSlackNotifications = srvSlackNotifications;
            _chronoBankCommandProducer = chronoBankCommandProducer;
        }

        private ChronobankApiClient Api => new ChronobankApiClient(_settings.BaseUri);

        public async Task<string> SetNewChronoBankContract(IWalletCredentials walletCredentials)
        {
            try
            {
                var contract = (await Api.ApiClientRegisterGetAsync()) as RegisterResponse;

                if (contract != null)
                    await _walletCredentialsRepository.SetChronoBankContract(walletCredentials.ClientId, contract.Contract);

                return contract?.Contract;
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(ChronoBankService), nameof(SetNewChronoBankContract), "", ex);

                var msg = $"ChronoBank contract was not set for {walletCredentials.ClientId}.\n{ex.Message}";
                await _srvSlackNotifications.SendNotification(ChannelTypes.Errors, msg, "lykkeapi");
            }

            return null;
        }

        public Task SendCashOutRequest(string id, string addressTo, double amount)
        {
            return _chronoBankCommandProducer.ProduceCashOutCommand(id, addressTo, amount);
        }
    }
}