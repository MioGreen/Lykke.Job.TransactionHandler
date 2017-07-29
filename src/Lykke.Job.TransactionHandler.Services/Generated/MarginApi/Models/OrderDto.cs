// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{
    public partial class OrderDto
    {
        /// <summary>
        /// Initializes a new instance of the OrderDto class.
        /// </summary>
        public OrderDto() { }

        /// <summary>
        /// Initializes a new instance of the OrderDto class.
        /// </summary>
        /// <param name="type">Possible values include: 'Buy', 'Sell'</param>
        /// <param name="status">Possible values include:
        /// 'WaitingForExecution', 'Active', 'Closed', 'Rejected'</param>
        /// <param name="closeReason">Possible values include: 'None',
        /// 'Close', 'StopLoss', 'TakeProfit', 'StopOut'</param>
        /// <param name="rejectReason">Possible values include: 'None',
        /// 'NoLiquidity', 'NotEnoughBalance', 'LeadToStopOut',
        /// 'AccountStopOut', 'InvalidExpectedOpenPrice', 'InvalidVolume',
        /// 'InvalidTakeProfit', 'InvalidStoploss', 'InvalidInstrument',
        /// 'InvalidAccount', 'NoTradingCondition'</param>
        public OrderDto(string id = default(string), string accountId = default(string), string instrument = default(string), string type = default(string), string status = default(string), string closeReason = default(string), string rejectReason = default(string), string rejectReasonText = default(string), double? expectedOpenPrice = default(double?), double? openPrice = default(double?), double? closePrice = default(double?), System.DateTime? openDate = default(System.DateTime?), System.DateTime? closeDate = default(System.DateTime?), double? volume = default(double?), double? matchedVolume = default(double?), double? matchedCloseVolume = default(double?), double? takeProfit = default(double?), double? stopLoss = default(double?), double? fpl = default(double?), System.Collections.Generic.IList<MatchedOrder> matchedOrders = default(System.Collections.Generic.IList<MatchedOrder>), System.Collections.Generic.IList<MatchedOrder> matchedCloseOrders = default(System.Collections.Generic.IList<MatchedOrder>))
        {
            Id = id;
            AccountId = accountId;
            Instrument = instrument;
            Type = type;
            Status = status;
            CloseReason = closeReason;
            RejectReason = rejectReason;
            RejectReasonText = rejectReasonText;
            ExpectedOpenPrice = expectedOpenPrice;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
            OpenDate = openDate;
            CloseDate = closeDate;
            Volume = volume;
            MatchedVolume = matchedVolume;
            MatchedCloseVolume = matchedCloseVolume;
            TakeProfit = takeProfit;
            StopLoss = stopLoss;
            Fpl = fpl;
            MatchedOrders = matchedOrders;
            MatchedCloseOrders = matchedCloseOrders;
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
        /// Gets possible values include: 'Buy', 'Sell'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }

        /// <summary>
        /// Gets or sets possible values include: 'WaitingForExecution',
        /// 'Active', 'Closed', 'Rejected'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'None', 'Close', 'StopLoss',
        /// 'TakeProfit', 'StopOut'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "closeReason")]
        public string CloseReason { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'None', 'NoLiquidity',
        /// 'NotEnoughBalance', 'LeadToStopOut', 'AccountStopOut',
        /// 'InvalidExpectedOpenPrice', 'InvalidVolume', 'InvalidTakeProfit',
        /// 'InvalidStoploss', 'InvalidInstrument', 'InvalidAccount',
        /// 'NoTradingCondition'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "rejectReason")]
        public string RejectReason { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "rejectReasonText")]
        public string RejectReasonText { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "expectedOpenPrice")]
        public double? ExpectedOpenPrice { get; set; }

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
        [Newtonsoft.Json.JsonProperty(PropertyName = "openDate")]
        public System.DateTime? OpenDate { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "closeDate")]
        public System.DateTime? CloseDate { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "volume")]
        public double? Volume { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "matchedVolume")]
        public double? MatchedVolume { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "matchedCloseVolume")]
        public double? MatchedCloseVolume { get; set; }

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

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "matchedOrders")]
        public System.Collections.Generic.IList<MatchedOrder> MatchedOrders { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "matchedCloseOrders")]
        public System.Collections.Generic.IList<MatchedOrder> MatchedCloseOrders { get; set; }

    }
}
