namespace Lykke.Job.TransactionHandler.Core.Services.MarginTrading
{
    public interface IMarginDataServiceResolver
    {
        IMarginDataService Resolve(bool isDemo);
    }
}