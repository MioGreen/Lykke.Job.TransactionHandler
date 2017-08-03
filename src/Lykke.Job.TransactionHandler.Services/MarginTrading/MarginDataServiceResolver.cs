using System;
using Lykke.Job.TransactionHandler.Core;
using Lykke.Job.TransactionHandler.Core.Services.MarginTrading;

namespace Lykke.Job.TransactionHandler.Services.MarginTrading
{
    public class MarginDataServiceResolver : IMarginDataServiceResolver
    {
        private readonly AppSettings.MarginTradingSettings _settings;

        public MarginDataServiceResolver(AppSettings.MarginTradingSettings settings)
        {
            _settings = settings;
        }

        public IMarginDataService Resolve(bool isDemo)
        {
            var serviceSettings = new MarginDataServiceSettings();

            if (isDemo)
            {
                serviceSettings.ApiKey = _settings.DemoApiKey;
                serviceSettings.BaseUri = new Uri(_settings.DemoApiRootUrl);
            }
            else
            {
                serviceSettings.ApiKey = _settings.ApiKey;
                serviceSettings.BaseUri = new Uri(_settings.ApiRootUrl);
            }

            return new MarginDataService(serviceSettings);
        }
    }
}