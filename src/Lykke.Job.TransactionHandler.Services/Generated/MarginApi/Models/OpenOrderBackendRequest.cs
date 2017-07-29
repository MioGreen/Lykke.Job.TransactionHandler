// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{
    public partial class OpenOrderBackendRequest
    {
        /// <summary>
        /// Initializes a new instance of the OpenOrderBackendRequest class.
        /// </summary>
        public OpenOrderBackendRequest() { }

        /// <summary>
        /// Initializes a new instance of the OpenOrderBackendRequest class.
        /// </summary>
        public OpenOrderBackendRequest(string clientId = default(string), NewOrderBackendContract order = default(NewOrderBackendContract))
        {
            ClientId = clientId;
            Order = order;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "order")]
        public NewOrderBackendContract Order { get; set; }

    }
}
