using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Services.Messages.Email
{
    public interface ISrvEmailsFacade
    {
        Task SendNoRefundDepositDoneMail(string email, double amount, string assetBcnId);

        Task SendNoRefundOCashOutMail(string email, double amount, string assetId, string bcnHash);

        Task SendTransferCompletedEmail(string email, string clientName, string assetId, double amountFiat,
            double amountLkk, double price, string srcHash);

        Task SendDirectTransferCompletedEmail(string email, string clientName, string assetId, double amount, string srcHash);

        Task SendSolarCashOutCompletedEmail(string email, string addressTo, double amount);
    }
}