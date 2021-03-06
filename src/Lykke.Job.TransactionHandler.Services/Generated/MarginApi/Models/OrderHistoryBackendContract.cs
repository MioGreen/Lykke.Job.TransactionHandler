// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{
    public partial class OrderHistoryBackendContract
    {
        /// <summary>
        /// Initializes a new instance of the OrderHistoryBackendContract
        /// class.
        /// </summary>
        public OrderHistoryBackendContract() { }

        /// <summary>
        /// Initializes a new instance of the OrderHistoryBackendContract
        /// class.
        /// </summary>
        /// <param name="type">Possible values include: 'Buy', 'Sell'</param>
        /// <param name="status">Possible values include:
        /// 'WaitingForExecution', 'Active', 'Closed', 'Rejected',
        /// 'Closing'</param>
        /// <param name="closeReason">Possible values include: 'None',
        /// 'Close', 'StopLoss', 'TakeProfit', 'StopOut', 'Canceled'</param>
        public OrderHistoryBackendContract(string id = default(string), string accountId = default(string), string instrument = default(string), int? assetAccuracy = default(int?), OrderDirection? type = default(OrderDirection?), OrderStatus? status = default(OrderStatus?), OrderCloseReason? closeReason = default(OrderCloseReason?), System.DateTime? openDate = default(System.DateTime?), System.DateTime? closeDate = default(System.DateTime?), double? openPrice = default(double?), double? closePrice = default(double?), double? volume = default(double?), double? takeProfit = default(double?), double? stopLoss = default(double?), double? totalPnl = default(double?), double? pnl = default(double?), double? interestRateSwap = default(double?), double? openCommission = default(double?), double? closeCommission = default(double?))
        {
            Id = id;
            AccountId = accountId;
            Instrument = instrument;
            AssetAccuracy = assetAccuracy;
            Type = type;
            Status = status;
            CloseReason = closeReason;
            OpenDate = openDate;
            CloseDate = closeDate;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
            Volume = volume;
            TakeProfit = takeProfit;
            StopLoss = stopLoss;
            TotalPnl = totalPnl;
            Pnl = pnl;
            InterestRateSwap = interestRateSwap;
            OpenCommission = openCommission;
            CloseCommission = closeCommission;
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
        /// Gets or sets possible values include: 'Buy', 'Sell'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "type")]
        public OrderDirection? Type { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'WaitingForExecution',
        /// 'Active', 'Closed', 'Rejected', 'Closing'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "status")]
        public OrderStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'None', 'Close', 'StopLoss',
        /// 'TakeProfit', 'StopOut', 'Canceled'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "closeReason")]
        public OrderCloseReason? CloseReason { get; set; }

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
        [Newtonsoft.Json.JsonProperty(PropertyName = "totalPnl")]
        public double? TotalPnl { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "pnl")]
        public double? Pnl { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "interestRateSwap")]
        public double? InterestRateSwap { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "openCommission")]
        public double? OpenCommission { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "closeCommission")]
        public double? CloseCommission { get; set; }

    }
}
