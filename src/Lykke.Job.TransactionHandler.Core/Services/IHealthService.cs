
namespace Lykke.Job.TransactionHandler.Core.Services
{
    public interface IHealthService
    {
        string GetHealthViolationMessage();
        string GetHealthWarningMessage();
    }
}