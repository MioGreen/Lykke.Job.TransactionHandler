// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.BitcoinCoreAPI.Models
{
    public partial class OffchainCommitmentInfo
    {
        /// <summary>
        /// Initializes a new instance of the OffchainCommitmentInfo class.
        /// </summary>
        public OffchainCommitmentInfo() { }

        /// <summary>
        /// Initializes a new instance of the OffchainCommitmentInfo class.
        /// </summary>
        public OffchainCommitmentInfo(System.DateTime? date = default(System.DateTime?), decimal? clientAmount = default(decimal?), decimal? hubAmount = default(decimal?), System.Guid? clientCommitment = default(System.Guid?), System.Guid? hubCommitment = default(System.Guid?))
        {
            Date = date;
            ClientAmount = clientAmount;
            HubAmount = hubAmount;
            ClientCommitment = clientCommitment;
            HubCommitment = hubCommitment;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "date")]
        public System.DateTime? Date { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientAmount")]
        public decimal? ClientAmount { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "hubAmount")]
        public decimal? HubAmount { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientCommitment")]
        public System.Guid? ClientCommitment { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "hubCommitment")]
        public System.Guid? HubCommitment { get; set; }

    }
}
