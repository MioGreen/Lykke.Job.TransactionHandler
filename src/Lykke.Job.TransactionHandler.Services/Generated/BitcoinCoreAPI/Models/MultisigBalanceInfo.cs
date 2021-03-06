// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.BitcoinCoreAPI.Models
{
    public partial class MultisigBalanceInfo
    {
        /// <summary>
        /// Initializes a new instance of the MultisigBalanceInfo class.
        /// </summary>
        public MultisigBalanceInfo() { }

        /// <summary>
        /// Initializes a new instance of the MultisigBalanceInfo class.
        /// </summary>
        public MultisigBalanceInfo(string multisig = default(string), decimal? clientAmount = default(decimal?), decimal? hubAmount = default(decimal?))
        {
            Multisig = multisig;
            ClientAmount = clientAmount;
            HubAmount = hubAmount;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "multisig")]
        public string Multisig { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientAmount")]
        public decimal? ClientAmount { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "hubAmount")]
        public decimal? HubAmount { get; set; }

    }
}
