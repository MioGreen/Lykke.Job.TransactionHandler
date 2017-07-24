// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace LkeServices.Generated.MarginApi.Models
{
    public partial class IMarginTradingAccountHistory
    {
        /// <summary>
        /// Initializes a new instance of the IMarginTradingAccountHistory
        /// class.
        /// </summary>
        public IMarginTradingAccountHistory() { }

        /// <summary>
        /// Initializes a new instance of the IMarginTradingAccountHistory
        /// class.
        /// </summary>
        /// <param name="type">Possible values include: 'Deposit', 'Withdraw',
        /// 'OrderClosed'</param>
        public IMarginTradingAccountHistory(string id = default(string), System.DateTime? date = default(System.DateTime?), string accountId = default(string), string clientId = default(string), double? amount = default(double?), double? balance = default(double?), string comment = default(string), string type = default(string))
        {
            Id = id;
            Date = date;
            AccountId = accountId;
            ClientId = clientId;
            Amount = amount;
            Balance = balance;
            Comment = comment;
            Type = type;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "date")]
        public System.DateTime? Date { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "amount")]
        public double? Amount { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "balance")]
        public double? Balance { get; private set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "comment")]
        public string Comment { get; private set; }

        /// <summary>
        /// Gets possible values include: 'Deposit', 'Withdraw', 'OrderClosed'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }

    }
}
