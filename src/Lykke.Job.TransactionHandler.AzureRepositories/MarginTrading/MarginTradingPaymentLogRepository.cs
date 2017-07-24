using System;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.TransactionHandler.Core.Domain.MarginTrading;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.TransactionHandler.AzureRepositories.MarginTrading
{
    public class MarginTradingPaymentLogEntity : TableEntity, IMarginTradingPaymentLog
    {
        public string ClientId { get; set; }
        public string AccountId { get; set; }
        public DateTime DateTime { get; set; }
        public double Amount { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }

        internal static string GeneratePartitionKey(string clientId)
        {
            return clientId;
        }


        public static MarginTradingPaymentLogEntity Create(IMarginTradingPaymentLog src)
        {
            return new MarginTradingPaymentLogEntity
            {
                PartitionKey = GeneratePartitionKey(src.ClientId),
                Amount = src.Amount,
                AccountId = src.AccountId,
                ClientId = src.ClientId,
                DateTime = src.DateTime,
                Message = src.Message,
                TransactionId = src.TransactionId,
                IsError = src.IsError
            };
        }

    }

    public class MarginTradingPaymentLogRepository : IMarginTradingPaymentLogRepository
    {
        private readonly INoSQLTableStorage<MarginTradingPaymentLogEntity> _tableStorage;

        public MarginTradingPaymentLogRepository(INoSQLTableStorage<MarginTradingPaymentLogEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task CreateAsync(IMarginTradingPaymentLog record)
        {
            var newEntity = MarginTradingPaymentLogEntity.Create(record);
            return _tableStorage.InsertAndGenerateRowKeyAsDateTimeAsync(newEntity, newEntity.DateTime);
        }
    }

}