// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Services.Generated.EthereumCoreApi.Models
{
    public partial class HashResponseWithId
    {
        /// <summary>
        /// Initializes a new instance of the HashResponseWithId class.
        /// </summary>
        public HashResponseWithId()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the HashResponseWithId class.
        /// </summary>
        public HashResponseWithId(System.Guid? operationId = default(System.Guid?), string hashHex = default(string))
        {
            OperationId = operationId;
            HashHex = hashHex;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "operationId")]
        public System.Guid? OperationId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "hashHex")]
        public string HashHex { get; set; }

    }
}
