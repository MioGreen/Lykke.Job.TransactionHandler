// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{
    public partial class RetryFailedRequest
    {
        /// <summary>
        /// Initializes a new instance of the RetryFailedRequest class.
        /// </summary>
        public RetryFailedRequest() { }

        /// <summary>
        /// Initializes a new instance of the RetryFailedRequest class.
        /// </summary>
        public RetryFailedRequest(System.Guid? transactionId = default(System.Guid?))
        {
            TransactionId = transactionId;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "transactionId")]
        public System.Guid? TransactionId { get; set; }

    }
}
