// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.BitcoinCoreAPI.Models
{
    public partial class OffchainClientBalanceResponse
    {
        /// <summary>
        /// Initializes a new instance of the OffchainClientBalanceResponse
        /// class.
        /// </summary>
        public OffchainClientBalanceResponse() { }

        /// <summary>
        /// Initializes a new instance of the OffchainClientBalanceResponse
        /// class.
        /// </summary>
        public OffchainClientBalanceResponse(decimal? amount = default(decimal?))
        {
            Amount = amount;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "amount")]
        public decimal? Amount { get; set; }

    }
}
