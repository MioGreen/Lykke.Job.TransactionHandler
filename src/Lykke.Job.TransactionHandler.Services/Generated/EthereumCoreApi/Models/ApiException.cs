// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Services.Generated.EthereumCoreApi.Models
{
    public partial class ApiException
    {
        /// <summary>
        /// Initializes a new instance of the ApiException class.
        /// </summary>
        public ApiException()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ApiException class.
        /// </summary>
        public ApiException(ApiError error = default(ApiError))
        {
            Error = error;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public ApiError Error { get; set; }

    }
}
