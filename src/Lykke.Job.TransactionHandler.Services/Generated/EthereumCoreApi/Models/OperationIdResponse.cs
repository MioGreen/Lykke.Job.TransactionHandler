// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Services.Generated.EthereumCoreApi.Models
{
    public partial class OperationIdResponse
    {
        /// <summary>
        /// Initializes a new instance of the OperationIdResponse class.
        /// </summary>
        public OperationIdResponse()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the OperationIdResponse class.
        /// </summary>
        public OperationIdResponse(string operationId = default(string))
        {
            OperationId = operationId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "operationId")]
        public string OperationId { get; set; }

    }
}
