// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.BitcoinCoreAPI.Models
{
    public partial class OffchainChannelInfo
    {
        /// <summary>
        /// Initializes a new instance of the OffchainChannelInfo class.
        /// </summary>
        public OffchainChannelInfo() { }

        /// <summary>
        /// Initializes a new instance of the OffchainChannelInfo class.
        /// </summary>
        public OffchainChannelInfo(System.Guid? channelId = default(System.Guid?), System.DateTime? date = default(System.DateTime?), decimal? clientAmount = default(decimal?), decimal? hubAmount = default(decimal?), string transactionHash = default(string), bool? actual = default(bool?))
        {
            ChannelId = channelId;
            Date = date;
            ClientAmount = clientAmount;
            HubAmount = hubAmount;
            TransactionHash = transactionHash;
            Actual = actual;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "channelId")]
        public System.Guid? ChannelId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "date")]
        public System.DateTime? Date { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientAmount")]
        public decimal? ClientAmount { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "hubAmount")]
        public decimal? HubAmount { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "transactionHash")]
        public string TransactionHash { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "actual")]
        public bool? Actual { get; set; }

    }
}
