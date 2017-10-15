﻿using System;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using AzureStorage.Blob;
using AzureStorage.Queue;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common;
using Common.Log;
using Lykke.Job.TransactionHandler.AzureRepositories.Assets;
using Lykke.Job.TransactionHandler.AzureRepositories.BitCoin;
using Lykke.Job.TransactionHandler.AzureRepositories.Blockchain;
using Lykke.Job.TransactionHandler.AzureRepositories.CashOperations;
using Lykke.Job.TransactionHandler.AzureRepositories.ChronoBank;
using Lykke.Job.TransactionHandler.AzureRepositories.Clients;
using Lykke.Job.TransactionHandler.AzureRepositories.Ethereum;
using Lykke.Job.TransactionHandler.AzureRepositories.Exchange;
using Lykke.Job.TransactionHandler.AzureRepositories.MarginTrading;
using Lykke.Job.TransactionHandler.AzureRepositories.Messages.Email;
using Lykke.Job.TransactionHandler.AzureRepositories.Offchain;
using Lykke.Job.TransactionHandler.AzureRepositories.PaymentSystems;
using Lykke.Job.TransactionHandler.AzureRepositories.Quanta;
using Lykke.Job.TransactionHandler.AzureRepositories.SolarCoin;
using Lykke.Job.TransactionHandler.Core.Domain.Assets;
using Lykke.Job.TransactionHandler.Core.Domain.BitCoin;
using Lykke.Job.TransactionHandler.Core.Domain.Blockchain;
using Lykke.Job.TransactionHandler.Core.Domain.CashOperations;
using Lykke.Job.TransactionHandler.Core.Domain.ChronoBank;
using Lykke.Job.TransactionHandler.Core.Domain.Clients;
using Lykke.Job.TransactionHandler.Core.Domain.Clients.Core.Clients;
using Lykke.Job.TransactionHandler.Core.Domain.Ethereum;
using Lykke.Job.TransactionHandler.Core.Domain.Exchange;
using Lykke.Job.TransactionHandler.Core.Domain.MarginTrading;
using Lykke.Job.TransactionHandler.Core.Domain.Messages.Email;
using Lykke.Job.TransactionHandler.Core.Domain.Offchain;
using Lykke.Job.TransactionHandler.Core.Domain.PaymentSystems;
using Lykke.Job.TransactionHandler.Core.Domain.Quanta;
using Lykke.Job.TransactionHandler.Core.Domain.SolarCoin;
using Lykke.Job.TransactionHandler.Core.Services;
using Lykke.Job.TransactionHandler.Core.Services.AppNotifications;
using Lykke.Job.TransactionHandler.Core.Services.BitCoin;
using Lykke.Job.TransactionHandler.Core.Services.BitCoin.BitCoinApi;
using Lykke.Job.TransactionHandler.Core.Services.ChronoBank;
using Lykke.Job.TransactionHandler.Core.Services.Ethereum;
using Lykke.Job.TransactionHandler.Core.Services.MarginTrading;
using Lykke.Job.TransactionHandler.Core.Services.Messages.Email;
using Lykke.Job.TransactionHandler.Core.Services.Messages.Email.Sender;
using Lykke.Job.TransactionHandler.Core.Services.Offchain;
using Lykke.Job.TransactionHandler.Core.Services.Quanta;
using Lykke.Job.TransactionHandler.Core.Services.SolarCoin;
using Lykke.Job.TransactionHandler.Queues;
using Lykke.Job.TransactionHandler.Services;
using Lykke.Job.TransactionHandler.Services.BitCoin;
using Lykke.Job.TransactionHandler.Services.BitCoin.BitCoinApiClient;
using Lykke.Job.TransactionHandler.Services.ChronoBank;
using Lykke.Job.TransactionHandler.Services.Ethereum;
using Lykke.EthereumCoreClient;
using Lykke.Job.TransactionHandler.Services.Http;
using Lykke.Job.TransactionHandler.Services.MarginTrading;
using Lykke.Job.TransactionHandler.Services.Messages.Email;
using Lykke.Job.TransactionHandler.Services.Notifications;
using Lykke.Job.TransactionHandler.Services.Offchain;
using Lykke.Job.TransactionHandler.Services.Quanta;
using Lykke.Job.TransactionHandler.Services.SolarCoin;
using Lykke.MatchingEngine.Connector.Services;
using Lykke.Service.Assets.Client.Custom;
using Lykke.Service.ClientAccount.Client.AutorestClient;
using Lykke.Service.ExchangeOperations.Client;
using Lykke.Service.OperationsRepository.Client;
using Lykke.Service.OperationsHistory.HistoryWriter.Abstractions;
using Lykke.Service.OperationsHistory.HistoryWriter.Implementation;
using Lykke.Service.PendingActions.Client;
using Lykke.SettingsReader;
using Lykke.Service.PersonalData.Client;
using Lykke.Service.PersonalData.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Job.TransactionHandler.Modules
{
    public class JobModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;
        private readonly IReloadingManager<AppSettings.TransactionHandlerSettings> _jobSettings;
        private readonly IReloadingManager<AppSettings.DbSettings> _dbSettings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;



        public JobModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _settings = settings;
            _jobSettings = _settings.Nested(x => x.TransactionHandlerJob);
            _dbSettings = _jobSettings.Nested(x => x.Db);
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_jobSettings.CurrentValue)
                .SingleInstance();

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(TimeSpan.FromSeconds(30)));

            // NOTE: You can implement your own poison queue notifier. See https://github.com/LykkeCity/JobTriggers/blob/master/readme.md
            // builder.Register<PoisionQueueNotifierImplementation>().As<IPoisionQueueNotifier>();

            _services.UseAssetsClient(new AssetServiceSettings
            {
                BaseUri = new Uri(_settings.CurrentValue.Assets.ServiceUrl),
                AssetPairsCacheExpirationPeriod = _jobSettings.CurrentValue.AssetsCache.ExpirationPeriod,
                AssetsCacheExpirationPeriod = _jobSettings.CurrentValue.AssetsCache.ExpirationPeriod
            });

            Mapper.Initialize(cfg => cfg.CreateMap<IBcnCredentialsRecord, BcnCredentialsRecordEntity>().IgnoreTableEntityFields());

            Mapper.Initialize(cfg => cfg.CreateMap<IEthereumTransactionRequest, EthereumTransactionReqEntity>().IgnoreTableEntityFields()
                .ForMember(x => x.SignedTransferVal, config => config.Ignore())
                .ForMember(x => x.OperationIdsVal, config => config.Ignore()));

            Mapper.Configuration.AssertConfigurationIsValid();

            BindRabbitMq(builder);
            BindMatchingEngineChannel(builder);
            BindRepositories(builder);
            BindServices(builder);
            BindCachedDicts(builder);

            builder.Populate(_services);
        }

        public static void BindCachedDicts(ContainerBuilder builder)
        {
            builder.Register(x =>
            {
                var ctx = x.Resolve<IComponentContext>();
                return new CachedDataDictionary<string, IAssetSetting>(
                    async () => (await ctx.Resolve<IAssetSettingRepository>().GetAssetSettings()).ToDictionary(itm => itm.Asset));
            }).SingleInstance();
        }

        private void BindMatchingEngineChannel(ContainerBuilder container)
        {
            var socketLog = new SocketLogDynamic(i => { },
                str => Console.WriteLine(DateTime.UtcNow.ToIsoDateTime() + ": " + str));

            container.BindMeClient(_settings.CurrentValue.MatchingEngineClient.IpEndpoint.GetClientIpEndPoint(), socketLog);
        }

        private void BindServices(ContainerBuilder builder)
        {
            builder.RegisterType<HttpRequestClient>().SingleInstance();
            builder.RegisterType<BitcoinApiClient>()
                .As<IBitcoinApiClient>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.BitCoinCore));
            builder.RegisterType<OffchainRequestService>().As<IOffchainRequestService>();
            builder.RegisterType<SrvSlackNotifications>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.SlackIntegration));

            var exchangeOperationsService = new ExchangeOperationsServiceClient(_jobSettings.CurrentValue.ExchangeOperationsServiceUrl);
            builder.RegisterInstance(exchangeOperationsService).As<IExchangeOperationsServiceClient>().SingleInstance();

            builder.Register<IAppNotifications>(x => new SrvAppNotifications(
                _settings.CurrentValue.AppNotifications.HubConnString,
                _settings.CurrentValue.AppNotifications.HubName));

            builder.RegisterType<ChronoBankService>().As<IChronoBankService>().SingleInstance();
            builder.RegisterType<SrvSolarCoinHelper>().As<ISrvSolarCoinHelper>().SingleInstance();
            builder.RegisterType<QuantaService>().As<IQuantaService>().SingleInstance();

            builder.Register<IEthereumApi>(x =>
            {
                var api = new EthereumApi(new Uri(_settings.CurrentValue.Ethereum.EthereumCoreUrl));
                api.SetRetryPolicy(null);
                return api;
            }).SingleInstance();

            builder.RegisterType<SrvEthereumHelper>().As<ISrvEthereumHelper>().SingleInstance();

            builder.RegisterType<MarginDataServiceResolver>()
                .As<IMarginDataServiceResolver>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.MarginTrading));

            builder.RegisterType<EmailSender>().As<IEmailSender>().SingleInstance();
            builder.RegisterType<SrvEmailsFacade>().As<ISrvEmailsFacade>().SingleInstance();

            builder.RegisterOperationsRepositoryClients(_settings.CurrentValue.OperationsRepositoryClient.ServiceUrl, _log,
                _settings.CurrentValue.OperationsRepositoryClient.RequestTimeout);

            builder.RegisterType<BitcoinTransactionService>().As<IBitcoinTransactionService>().SingleInstance();
            var historyWriter = new HistoryWriter(_dbSettings.CurrentValue.HistoryLogsConnString, _log);
            builder.RegisterInstance(historyWriter).As<IHistoryWriter>();

            builder.RegisterPendingActionsClient(_settings.CurrentValue.PendingActionsClient.ServiceUrl, _log);

            builder.RegisterType<PersonalDataService>()
                .As<IPersonalDataService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.PersonalDataServiceSettings));
                
            builder.RegisterType<ClientAccountService>()
                .As<IClientAccountService>()
                .WithParameter("baseUri", new Uri(_settings.CurrentValue.ClientAccountClient.ServiceUrl));

        }

        private void BindRepositories(ContainerBuilder builder)
        {
            builder.RegisterInstance<IAssetSettingRepository>(new AssetSettingRepository(
                AzureTableStorage<AssetSettingEntity>.Create(_dbSettings.ConnectionString(x => x.DictsConnString),
                    "AssetSettings", _log)));

            builder.RegisterInstance<IBitcoinCommandSender>(new BitcoinCommandSender(
                AzureQueueExt.Create(_dbSettings.ConnectionString(x => x.BitCoinQueueConnectionString),
                    "intransactions")));

            builder.RegisterInstance<IBitCoinTransactionsRepository>(new BitCoinTransactionsRepository(
                AzureTableStorage<BitCoinTransactionEntity>.Create(
                    _dbSettings.ConnectionString(x => x.BitCoinQueueConnectionString), "BitCoinTransactions", _log)));

            builder.RegisterInstance<IWalletCredentialsRepository>(new WalletCredentialsRepository(
                AzureTableStorage<WalletCredentialsEntity>.Create(
                    _dbSettings.ConnectionString(x => x.ClientPersonalInfoConnString), "WalletCredentials", _log)));

            builder.RegisterInstance<IBcnClientCredentialsRepository>(new BcnClientCredentialsRepository(
                AzureTableStorage<BcnCredentialsRecordEntity>.Create(
                    _dbSettings.ConnectionString(x => x.ClientPersonalInfoConnString), "BcnClientCredentials", _log)));

            builder.RegisterInstance<ILimitTradeEventsRepository>(new LimitTradeEventsRepository(
                AzureTableStorage<LimitTradeEventEntity>.Create(
                    _dbSettings.ConnectionString(x => x.ClientPersonalInfoConnString), "LimitTradeEvents", _log)));

            builder.RegisterInstance<IForwardWithdrawalRepository>(new ForwardWithdrawalRepository(
                AzureTableStorage<ForwardWithdrawalEntity>.Create(
                    _dbSettings.ConnectionString(x => x.BalancesInfoConnString), "ForwardWithdrawal", _log)));

            builder.RegisterInstance<IChronoBankCommandProducer>(new SrvChronoBankCommandProducer(
                AzureQueueExt.Create(_dbSettings.ConnectionString(x => x.ChronoBankSrvConnString), "chronobank-out")));
            
            builder.RegisterInstance<IClientSettingsRepository>(new ClientSettingsRepository(
                AzureTableStorage<ClientSettingsEntity>.Create(
                    _dbSettings.ConnectionString(x => x.ClientPersonalInfoConnString), "TraderSettings", _log)));

            builder.RegisterInstance<IClientCacheRepository>(new ClientCacheRepository(
                AzureTableStorage<ClientCacheEntity>.Create(
                    _dbSettings.ConnectionString(x => x.ClientPersonalInfoConnString), "ClientCache", _log)));

            builder.RegisterInstance<IEthClientEventLogs>(new EthClientEventLogs(
                AzureTableStorage<EthClientEventRecord>.Create(_dbSettings.ConnectionString(x => x.LwEthLogsConnString),
                    "EthClientEventLogs", _log)));

            builder.RegisterInstance<IEthereumTransactionRequestRepository>(
                new EthereumTransactionRequestRepository(AzureTableStorage<EthereumTransactionReqEntity>.Create(
                    _dbSettings.ConnectionString(x => x.BitCoinQueueConnectionString), "EthereumTxRequest", _log)));

            builder.RegisterInstance<IMarketOrdersRepository>(new MarketOrdersRepository(
                AzureTableStorage<MarketOrderEntity>.Create(
                    _dbSettings.ConnectionString(x => x.HMarketOrdersConnString), "MarketOrders", _log)));

            builder.RegisterInstance<ILimitOrdersRepository>(new LimitOrdersRepository(
                AzureTableStorage<LimitOrderEntity>.Create(_dbSettings.ConnectionString(x => x.HMarketOrdersConnString),
                    "LimitOrders", _log)));

            builder.RegisterInstance<IMarginTradingPaymentLogRepository>(
                new MarginTradingPaymentLogRepository(AzureTableStorage<MarginTradingPaymentLogEntity>.Create(
                    _dbSettings.ConnectionString(x => x.LogsConnString), "MarginTradingPaymentsLog", _log)));

            builder.RegisterInstance<IEmailCommandProducer>(new EmailCommandProducer(
                AzureQueueExt.Create(_dbSettings.ConnectionString(x => x.ClientPersonalInfoConnString),
                    "emailsqueue")));

            builder.RegisterInstance<IOffchainOrdersRepository>(new OffchainOrderRepository(
                AzureTableStorage<OffchainOrder>.Create(_dbSettings.ConnectionString(x => x.OffchainConnString),
                    "OffchainOrders", _log)));

            builder.RegisterInstance<IOffchainRequestRepository>(new OffchainRequestRepository(
                AzureTableStorage<OffchainRequestEntity>.Create(_dbSettings.ConnectionString(x => x.OffchainConnString),
                    "OffchainRequests", _log)));

            builder.RegisterInstance<IOffchainTransferRepository>(new OffchainTransferRepository(
                AzureTableStorage<OffchainTransferEntity>.Create(
                    _dbSettings.ConnectionString(x => x.OffchainConnString), "OffchainTransfers", _log)));

            builder.RegisterInstance<IPaymentTransactionsRepository>(new PaymentTransactionsRepository(
                AzureTableStorage<PaymentTransactionEntity>.Create(
                    _dbSettings.ConnectionString(x => x.ClientPersonalInfoConnString), "PaymentTransactions", _log),
                AzureTableStorage<AzureMultiIndex>.Create(
                    _dbSettings.ConnectionString(x => x.ClientPersonalInfoConnString), "PaymentTransactions", _log)));

            builder.RegisterInstance<IQuantaCommandProducer>(new SrvQuantaCommandProducer(
                AzureQueueExt.Create(_dbSettings.ConnectionString(x => x.QuantaSrvConnString), "quanta-out")));

            builder.RegisterInstance<ISrvSolarCoinCommandProducer>(new SrvSolarCoinCommandProducer(
                AzureQueueExt.Create(_dbSettings.ConnectionString(x => x.SolarCoinConnString), "solar-out")));

            builder.RegisterInstance(new BitcoinTransactionContextBlobStorage(
                    AzureBlobStorage.Create(_dbSettings.ConnectionString(x => x.BitCoinQueueConnectionString))))
                .As<IBitcoinTransactionContextBlobStorage>();
        }

        private void BindRabbitMq(ContainerBuilder builder)
        {
            builder.RegisterInstance(_settings.CurrentValue.RabbitMq);
            builder.RegisterType<CashInOutQueue>().SingleInstance();
            builder.RegisterType<TransferQueue>().SingleInstance();
            builder.RegisterType<LimitTradeQueue>().SingleInstance().WithParameter(TypedParameter.From(_settings.CurrentValue.Ethereum));
            builder.RegisterType<TradeQueue>().SingleInstance().WithParameter(TypedParameter.From(_settings.CurrentValue.Ethereum));
            builder.RegisterType<EthereumEventsQueue>().SingleInstance();
        }
    }
}