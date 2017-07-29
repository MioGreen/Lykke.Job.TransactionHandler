// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{
    public partial class OrderHistoryDto
    {
        /// <summary>
        /// Initializes a new instance of the OrderHistoryDto class.
        /// </summary>
        public OrderHistoryDto() { }

        /// <summary>
        /// Initializes a new instance of the OrderHistoryDto class.
        /// </summary>
        /// <param name="type">Possible values include: 'Buy', 'Sell'</param>
        /// <param name="closeReason">Possible values include: 'None',
        /// 'Close', 'StopLoss', 'TakeProfit', 'StopOut'</param>
        public OrderHistoryDto(string id = default(string), string accountId = default(string), string instrument = default(string), int? assetAccuracy = default(int?), string type = default(string), string closeReason = default(string), System.DateTime? openDate = default(System.DateTime?), System.DateTime? closeDate = default(System.DateTime?), double? openPrice = default(double?), double? closePrice = default(double?), double? volume = default(double?), double? takeProfit = default(double?), double? stopLoss = default(double?), double? fpl = default(double?))
        {
            Id = id;
            AccountId = accountId;
            Instrument = instrument;
            AssetAccuracy = assetAccuracy;
            Type = type;
            CloseReason = closeReason;
            OpenDate = openDate;
            CloseDate = closeDate;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
            Volume = volume;
            TakeProfit = takeProfit;
            StopLoss = stopLoss;
            Fpl = fpl;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "instrument")]
        public string Instrument { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "assetAccuracy")]
        public int? AssetAccuracy { get; set; }

        /// <summary>
        /// Gets possible values include: 'Buy', 'Sell'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }

        /// <summary>
        /// Gets or sets possible values include: 'None', 'Close', 'StopLoss',
        /// 'TakeProfit', 'StopOut'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "closeReason")]
        public string CloseReason { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "openDate")]
        public System.DateTime? OpenDate { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "closeDate")]
        public System.DateTime? CloseDate { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "openPrice")]
        public double? OpenPrice { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "closePrice")]
        public double? ClosePrice { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "volume")]
        public double? Volume { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "takeProfit")]
        public double? TakeProfit { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "stopLoss")]
        public double? StopLoss { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "fpl")]
        public double? Fpl { get; set; }

    }
}
