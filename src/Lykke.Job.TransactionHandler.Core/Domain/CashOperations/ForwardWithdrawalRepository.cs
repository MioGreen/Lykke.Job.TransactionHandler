using System;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.CashOperations
{
    public interface IForwardWithdrawal
    {
        string Id { get; }
        string AssetId { get; }
        string ClientId { get; }
        double Amount { get; }
        DateTime DateTime { get; }

        string CashInId { get; }
    }

    public class ForwardWithdrawal : IForwardWithdrawal
    {
        public string Id { get; set; }
        public string AssetId { get; set; }
        public string ClientId { get; set; }
        public double Amount { get; set; }
        public DateTime DateTime { get; set; }
        public string CashInId { get; set; }

        public static ForwardWithdrawal Create(string assetId, double amount, string clientId)
        {
            return new ForwardWithdrawal
            {
                AssetId = assetId,
                Amount = amount,
                ClientId = clientId,
                DateTime = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString()
            };
        }
    }

    public interface IForwardWithdrawalRepository
    {
        Task<string> InsertAsync(IForwardWithdrawal forwardWithdrawal);
        Task SetLinkedCashInOperationId(string clientId, string id, string cashInId);
    }
}