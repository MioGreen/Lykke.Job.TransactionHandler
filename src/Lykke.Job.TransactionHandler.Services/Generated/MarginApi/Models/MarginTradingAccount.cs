// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{
    public partial class MarginTradingAccount
    {
        /// <summary>
        /// Initializes a new instance of the MarginTradingAccount class.
        /// </summary>
        public MarginTradingAccount() { }

        /// <summary>
        /// Initializes a new instance of the MarginTradingAccount class.
        /// </summary>
        public MarginTradingAccount(string id = default(string), string clientId = default(string), string tradingConditionId = default(string), string baseAssetId = default(string), double? balance = default(double?), double? withdrawTransferLimit = default(double?))
        {
            Id = id;
            ClientId = clientId;
            TradingConditionId = tradingConditionId;
            BaseAssetId = baseAssetId;
            Balance = balance;
            WithdrawTransferLimit = withdrawTransferLimit;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "tradingConditionId")]
        public string TradingConditionId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "baseAssetId")]
        public string BaseAssetId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "balance")]
        public double? Balance { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "withdrawTransferLimit")]
        public double? WithdrawTransferLimit { get; set; }

    }
}
