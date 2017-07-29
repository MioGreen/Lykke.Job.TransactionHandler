// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{
    public partial class MarginTradingAsset
    {
        /// <summary>
        /// Initializes a new instance of the MarginTradingAsset class.
        /// </summary>
        public MarginTradingAsset() { }

        /// <summary>
        /// Initializes a new instance of the MarginTradingAsset class.
        /// </summary>
        public MarginTradingAsset(string id = default(string), string name = default(string), string baseAssetId = default(string), string quoteAssetId = default(string), int? accuracy = default(int?))
        {
            Id = id;
            Name = name;
            BaseAssetId = baseAssetId;
            QuoteAssetId = quoteAssetId;
            Accuracy = accuracy;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "baseAssetId")]
        public string BaseAssetId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "quoteAssetId")]
        public string QuoteAssetId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "accuracy")]
        public int? Accuracy { get; set; }

    }
}
