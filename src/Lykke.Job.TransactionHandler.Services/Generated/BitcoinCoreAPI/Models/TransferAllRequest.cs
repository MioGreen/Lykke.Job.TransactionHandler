// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.BitcoinCoreAPI.Models
{
    public partial class TransferAllRequest
    {
        /// <summary>
        /// Initializes a new instance of the TransferAllRequest class.
        /// </summary>
        public TransferAllRequest() { }

        /// <summary>
        /// Initializes a new instance of the TransferAllRequest class.
        /// </summary>
        public TransferAllRequest(System.Guid? transactionId = default(System.Guid?), string sourceAddress = default(string), string destinationAddress = default(string))
        {
            TransactionId = transactionId;
            SourceAddress = sourceAddress;
            DestinationAddress = destinationAddress;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "transactionId")]
        public System.Guid? TransactionId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "sourceAddress")]
        public string SourceAddress { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "destinationAddress")]
        public string DestinationAddress { get; set; }

    }
}
