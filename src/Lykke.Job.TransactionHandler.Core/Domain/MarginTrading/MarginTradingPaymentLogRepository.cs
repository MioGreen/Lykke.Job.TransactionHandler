using System;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.MarginTrading
{
    public interface IMarginTradingPaymentLog
    {
        string ClientId { get; set; }
        string AccountId { get; set; }
        DateTime DateTime { get; set; }
        double Amount { get; set; }
        string TransactionId { get; set; }
        string Message { get; set; }
        bool IsError { get; set; }
    }

    public class MarginTradingPaymentLog : IMarginTradingPaymentLog
    {
        public string ClientId { get; set; }
        public string AccountId { get; set; }
        public DateTime DateTime { get; set; }
        public double Amount { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }

        public static MarginTradingPaymentLog CreateOk(string clientId, string accountId, DateTime dateTime,
            double amount, string transactionId)
        {
            return new MarginTradingPaymentLog
            {
                ClientId = clientId,
                AccountId = accountId,
                DateTime = dateTime,
                Amount = amount,
                TransactionId = transactionId
            };
        }

        public static MarginTradingPaymentLog CreateError(string clientId, string accountId, DateTime dateTime,
            double amount, string message)
        {
            return new MarginTradingPaymentLog
            {
                ClientId = clientId,
                AccountId = accountId,
                DateTime = dateTime,
                Amount = amount,
                IsError = true,
                Message = message
            };
        }
    }

    public interface IMarginTradingPaymentLogRepository
    {
        Task CreateAsync(IMarginTradingPaymentLog record);
    }
}