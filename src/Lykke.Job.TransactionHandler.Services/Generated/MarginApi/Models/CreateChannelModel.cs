// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{
    public partial class CreateChannelModel
    {
        /// <summary>
        /// Initializes a new instance of the CreateChannelModel class.
        /// </summary>
        public CreateChannelModel() { }

        /// <summary>
        /// Initializes a new instance of the CreateChannelModel class.
        /// </summary>
        public CreateChannelModel(string clientPubKey = default(string), string hotWalletPubKey = default(string), double? clientAmount = default(double?), double? hubAmount = default(double?), string asset = default(string))
        {
            ClientPubKey = clientPubKey;
            HotWalletPubKey = hotWalletPubKey;
            ClientAmount = clientAmount;
            HubAmount = hubAmount;
            Asset = asset;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientPubKey")]
        public string ClientPubKey { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "hotWalletPubKey")]
        public string HotWalletPubKey { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientAmount")]
        public double? ClientAmount { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "hubAmount")]
        public double? HubAmount { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "asset")]
        public string Asset { get; set; }

    }
}
