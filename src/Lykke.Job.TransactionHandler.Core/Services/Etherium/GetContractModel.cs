namespace Lykke.Job.TransactionHandler.Core.Services.Etherium
{
    public class GetContractModel
    {
        public string Contract { get; set; }
    }

    public class OperationResponse
    {
        public string OperationId { get; set; }
    }

    public class EstimationResponse
    {
        public bool IsAllowed { get; set; }
        public string GasAmount { get; set; }
    }
}