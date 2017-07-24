// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace LkeServices.Generated.MarginApi.Models
{
    public partial class NewOrderBackendContract
    {
        /// <summary>
        /// Initializes a new instance of the NewOrderBackendContract class.
        /// </summary>
        public NewOrderBackendContract() { }

        /// <summary>
        /// Initializes a new instance of the NewOrderBackendContract class.
        /// </summary>
        /// <param name="fillType">Possible values include: 'FillOrKill',
        /// 'PartialFill'</param>
        public NewOrderBackendContract(string accountId = default(string), string instrument = default(string), double? expectedOpenPrice = default(double?), double? volume = default(double?), double? takeProfit = default(double?), double? stopLoss = default(double?), OrderFillType? fillType = default(OrderFillType?))
        {
            AccountId = accountId;
            Instrument = instrument;
            ExpectedOpenPrice = expectedOpenPrice;
            Volume = volume;
            TakeProfit = takeProfit;
            StopLoss = stopLoss;
            FillType = fillType;
        }

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
        [Newtonsoft.Json.JsonProperty(PropertyName = "expectedOpenPrice")]
        public double? ExpectedOpenPrice { get; set; }

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
        /// Gets or sets possible values include: 'FillOrKill', 'PartialFill'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "fillType")]
        public OrderFillType? FillType { get; set; }

    }
}
