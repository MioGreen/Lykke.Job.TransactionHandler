using System.Linq;
using System.Net;

namespace Lykke.Job.TransactionHandler.Core
{
    public class AppSettings
    {
        public TransactionHandlerSettings TransactionHandlerJob { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }

        public class TransactionHandlerSettings
        {
            public DbSettings Db { get; set; }
            public EthereumSettings EthereumSettings { get; set; }
            public SlackIntegrationSettings SlackIntegration { get; set; }
            public BitcoinCoreSettings BitCoinCore { get; set; }
            public SolarCoinSettings SolarCoin { get; set; }
            public MarginSettings MarginSettings { get; set; }
            public MatchingEngineSettings MatchingEngine { get; set; }
            public NotificationsSettings Notifications { get; set; }
            public ChronoBankSettings ChronoBankSettings { get; set; }
            public QuantaSettings QuantaSettings { get; set; }
            public string ExchangeOperationsServiceUrl { get; set; }
        }

        public class DbSettings
        {
            public string LogsConnString { get; set; }
            public string BitCoinQueueConnectionString { get; set; }
            public string DictsConnString { get; set; }
            public string ClientPersonalInfoConnString { get; set; }
            public string BalancesInfoConnString { get; set; }
            public string HTradesConnString { get; set; }
            public string ChronoBankSrvConnString { get; set; }
            public string LwEthLogsConnString { get; set; }
            public string HMarketOrdersConnString { get; set; }
            public string OffchainConnString { get; set; }
            public string QuantaSrvConnString { get; set; }
            public string SolarCoinConnString { get; set; }
        }

        public class QuantaSettings
        {
            public string ApiUrl { get; set; }
        }

        public class ChronoBankSettings
        {
            public string ApiUrl { get; set; }
        }

        public class NotificationsSettings
        {
            public string HubConnectionString { get; set; }
            public string HubName { get; set; }
        }

        public class SolarCoinSettings
        {
            public string GetAddressUrl { get; set; }
        }

        public class MatchingEngineSettings
        {
            public IpEndpointSettings IpEndpoint { get; set; }

            public RabbitMqSettings RabbitMq { get; set; }

            public string MetricLoggerLine { get; set; }
        }
        
        public class IpEndpointSettings
        {
            public string InternalHost { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }

            public IPEndPoint GetClientIpEndPoint(bool useInternal = false)
            {
                return new IPEndPoint(IPAddress.Parse(useInternal ? InternalHost : Host), Port);
            }

            public IPEndPoint GetServerIpEndPoint()
            {
                return new IPEndPoint(IPAddress.Any, Port);
            }

        }

        public class MarginSettings
        {
            public string MarginTradingWampHost { get; set; }
            public string WampBackendUrl { get; set; }
            public string ApiKey { get; set; }
            public string DemoApiKey { get; set; }
            public string ApiUrl { get; set; }
            public string ApiRootUrl { get; set; }
            public string DemoApiRootUrl { get; set; }
            public string RpcToken { get; set; }
            public bool RemoteConsoleEnabled { get; set; }
            public bool AutoCreateAccounts { get; set; }
            public Queues RabbitMqQueues { get; set; }
            public RabbitMqConnectionSettings RabbitMqSettings { get; set; }
            public RabbitMqConnectionSettings MarginTradingRabbitMqSettings { get; set; }
            public EmailServiceBus EmailSettings { get; set; }
            public string LogsConnectionString { get; set; }
            public string MoneyTrasferClientId { get; set; }

            public class Queues
            {
                public string AccountHistoryQueue { get; set; }
                public string OrderHistoryQueue { get; set; }
                public string OrderRejectedQueue { get; set; }
                public string OrderbookPricesQueue { get; set; }
                public string OrderChangedQueue { get; set; }
                public string AccountChangedQueue { get; set; }
                public string AccountStopoutQueue { get; set; }
                public string UserUpdatesQueue { get; set; }
            }
        }

        public class EmailServiceBus
        {
            public string Key { get; set; }
            public string QueueName { get; set; }
            public string NamespaceUrl { get; set; }
            public string PolicyName { get; set; }
        }

        public class RabbitMqConnectionSettings
        {
            public string InternalConnectionString { get; set; }
            public string ConnectionString { get; set; }
            public string QueueName { get; set; }
            public string ExchangeName { get; set; }
        }

        public class BitcoinCoreSettings
        {
            public string BitcoinCoreApiUrl { get; set; }
            public string ClientSigningApiUrl { get; set; }
        }

        public class SlackIntegrationSettings
        {
            public class Channel
            {
                public string Type { get; set; }
                public string WebHookUrl { get; set; }
            }

            public string Env { get; set; }
            public Channel[] Channels { get; set; }

            public string GetChannelWebHook(string type)
            {
                return Channels.FirstOrDefault(x => x.Type == type)?.WebHookUrl;
            }
        }

        public class EthereumSettings
        {
            public string EthereumCoreUrl { get; set; }
            public string HotwalletAddress { get; set; }
        }

        public class RabbitMqSettings
        {
            //public string Host { get; set; }
            public string ExternalHost { get; set; }

            public int Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }

            public string ExchangeOrderbook { get; set; }
            public string ExchangeSwap { get; set; }

            public string ExchangeCashOperation { get; set; }
            public string ExchangeTransfer { get; set; }
            public string ExchangeSwapOperation { get; set; }

            public string ExchangeEthereumCashIn { get; set; }
        }

        public class SlackNotificationsSettings
        {
            public AzureQueueSettings AzureQueue { get; set; }

            public int ThrottlingLimitSeconds { get; set; }
        }

        public class AzureQueueSettings
        {
            public string ConnectionString { get; set; }

            public string QueueName { get; set; }
        }
    }
}