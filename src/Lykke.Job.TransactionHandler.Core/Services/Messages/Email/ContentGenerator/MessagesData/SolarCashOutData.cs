namespace Lykke.Job.TransactionHandler.Core.Services.Messages.Email.ContentGenerator.MessagesData
{
    public class SolarCashOutData : IEmailMessageData
    {
        public string AddressTo { get; set; }
        public double Amount { get; set; }

        public string MessageId()
        {
            return "SolarCashOut";
        }
    }
}