// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace LkeServices.Generated.MarginApi.Models
{
    public partial class AccountHistoryBackendContract
    {
        /// <summary>
        /// Initializes a new instance of the AccountHistoryBackendContract
        /// class.
        /// </summary>
        public AccountHistoryBackendContract() { }

        /// <summary>
        /// Initializes a new instance of the AccountHistoryBackendContract
        /// class.
        /// </summary>
        /// <param name="type">Possible values include: 'Deposit', 'Withdraw',
        /// 'OrderClosed', 'Reset'</param>
        public AccountHistoryBackendContract(string id = default(string), System.DateTime? date = default(System.DateTime?), string accountId = default(string), string clientId = default(string), double? amount = default(double?), double? balance = default(double?), double? withdrawTransferLimit = default(double?), string comment = default(string), AccountHistoryType? type = default(AccountHistoryType?))
        {
            Id = id;
            Date = date;
            AccountId = accountId;
            ClientId = clientId;
            Amount = amount;
            Balance = balance;
            WithdrawTransferLimit = withdrawTransferLimit;
            Comment = comment;
            Type = type;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "date")]
        public System.DateTime? Date { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "amount")]
        public double? Amount { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "balance")]
        public double? Balance { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "withdrawTransferLimit")]
        public double? WithdrawTransferLimit { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Deposit', 'Withdraw',
        /// 'OrderClosed', 'Reset'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "type")]
        public AccountHistoryType? Type { get; set; }

    }
}
