// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Services.Generated.EthereumCoreApi.Models
{
    public partial class CheckIdResponse
    {
        /// <summary>
        /// Initializes a new instance of the CheckIdResponse class.
        /// </summary>
        public CheckIdResponse()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CheckIdResponse class.
        /// </summary>
        public CheckIdResponse(bool? isOk = default(bool?), System.Guid? proposedId = default(System.Guid?))
        {
            IsOk = isOk;
            ProposedId = proposedId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isOk")]
        public bool? IsOk { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "proposedId")]
        public System.Guid? ProposedId { get; set; }

    }
}
