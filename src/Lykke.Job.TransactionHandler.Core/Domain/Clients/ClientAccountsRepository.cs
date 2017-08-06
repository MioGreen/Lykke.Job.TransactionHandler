using System;
using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Clients
{
    public interface IClientAccount
    {
        DateTime Registered { get; }
        string Id { get; }
        string Email { get; }
        string PartnerId { get; }        
        string Pin { get; }
        string NotificationsId { get; }

        /// <summary>
        /// If true, than this account is used for IOS review and may have some exceptional requirements
        /// </summary>
        bool IsReviewAccount { get; }
    }

    public class ClientAccount : IClientAccount
    {
        public DateTime Registered { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }        
        public string Pin { get; set; }
        public string NotificationsId { get; set; }
        public string PartnerId { get; set; }
        public bool IsReviewAccount { get; set; }
    }

    public interface IClientAccountsRepository
    {
        Task<IClientAccount> GetByIdAsync(string id);
    }
}