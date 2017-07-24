using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Messages.Email
{
    public interface IEmailCommandProducer
    {
        Task ProduceSendEmailCommand<T>(string mailAddress, T msgData);
    }
}