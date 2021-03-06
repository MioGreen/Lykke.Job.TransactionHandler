// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.BitcoinCoreAPI.Models
{
    public partial class IssueRequest
    {
        /// <summary>
        /// Initializes a new instance of the IssueRequest class.
        /// </summary>
        public IssueRequest() { }

        /// <summary>
        /// Initializes a new instance of the IssueRequest class.
        /// </summary>
        public IssueRequest(System.Guid? transactionId = default(System.Guid?), string address = default(string), string asset = default(string), decimal? amount = default(decimal?))
        {
            TransactionId = transactionId;
            Address = address;
            Asset = asset;
            Amount = amount;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "transactionId")]
        public System.Guid? TransactionId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "asset")]
        public string Asset { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "amount")]
        public decimal? Amount { get; set; }

    }
}
