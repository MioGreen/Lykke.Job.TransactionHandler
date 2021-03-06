// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{
    public partial class OrderBook
    {
        /// <summary>
        /// Initializes a new instance of the OrderBook class.
        /// </summary>
        public OrderBook() { }

        /// <summary>
        /// Initializes a new instance of the OrderBook class.
        /// </summary>
        public OrderBook(string instrument = default(string), System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<LimitOrder>> buy = default(System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<LimitOrder>>), System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<LimitOrder>> sell = default(System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<LimitOrder>>))
        {
            Instrument = instrument;
            Buy = buy;
            Sell = sell;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "instrument")]
        public string Instrument { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "buy")]
        public System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<LimitOrder>> Buy { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "sell")]
        public System.Collections.Generic.IDictionary<string, System.Collections.Generic.IList<LimitOrder>> Sell { get; set; }

    }
}
