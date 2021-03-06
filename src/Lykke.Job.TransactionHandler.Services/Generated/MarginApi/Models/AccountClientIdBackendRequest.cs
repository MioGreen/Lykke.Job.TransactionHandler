// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{
    public partial class AccountClientIdBackendRequest
    {
        /// <summary>
        /// Initializes a new instance of the AccountClientIdBackendRequest
        /// class.
        /// </summary>
        public AccountClientIdBackendRequest() { }

        /// <summary>
        /// Initializes a new instance of the AccountClientIdBackendRequest
        /// class.
        /// </summary>
        public AccountClientIdBackendRequest(string accountId = default(string), string clientId = default(string))
        {
            AccountId = accountId;
            ClientId = clientId;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }

    }
}
