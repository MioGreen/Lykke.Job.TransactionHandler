namespace Lykke.Job.TransactionHandler.Core.Services.Messages.Email.ContentGenerator.MessagesData
{
    public class NoRefundDepositDoneData : IEmailMessageData
    {
        public string AssetBcnId { get; set; }
        public double Amount { get; set; }
        public string MessageId()
        {
            return "NoRefundDepositDoneEmail";
        }
    }
}