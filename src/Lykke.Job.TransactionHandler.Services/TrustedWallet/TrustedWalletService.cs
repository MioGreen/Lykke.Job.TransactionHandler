using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.TransactionHandler.Core.Services.TrustedWallet;
using Lykke.MatchingEngine.Connector.Abstractions.Services;

namespace Lykke.Job.TransactionHandler.Services.TrustedWallet
{
    public class TrustedWalletService : ITrustedWalletService
    {
        private readonly ILog _log;
        private readonly IMatchingEngineClient _matchingEngineClient;

        public TrustedWalletService(IMatchingEngineClient matchingEngineClient, ILog log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _matchingEngineClient = matchingEngineClient ?? throw new ArgumentNullException(nameof(matchingEngineClient));
        }

        public async Task Deposit(string walletId, string assetId, decimal amount)
        {
            var id = GetNextRequestId();
            var result = await _matchingEngineClient.CashInOutAsync(id, walletId, assetId, (double)amount);

            if (result == null || result.Status != MatchingEngine.Connector.Abstractions.Models.MeStatusCodes.Ok)
            {
                await
                    _log.WriteWarningAsync(nameof(TrustedWalletService), nameof(Deposit), "ME error",
                        result.ToJson());
            }
        }

        private string GetNextRequestId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
