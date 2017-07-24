
namespace Lykke.Job.TransactionHandler.Models
{
    public class IsAliveResponse
    {
        public string Version { get; set; }
        public string Env { get; set; }
        public string HealthWarning { get; set; }

    }
}