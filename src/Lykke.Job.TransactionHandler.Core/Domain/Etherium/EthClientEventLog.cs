using System.Threading.Tasks;

namespace Lykke.Job.TransactionHandler.Core.Domain.Etherium
{
    public enum Event
    {
        Error
    }

    public interface IEthClientEventLogs
    {
        Task WriteEvent(string clientId, Event eventType, string data);
    }
}