// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace LkeServices.Generated.MarginApi.Models
{
    public partial class AggregatedOrderBookItemBackendContract
    {
        /// <summary>
        /// Initializes a new instance of the
        /// AggregatedOrderBookItemBackendContract class.
        /// </summary>
        public AggregatedOrderBookItemBackendContract() { }

        /// <summary>
        /// Initializes a new instance of the
        /// AggregatedOrderBookItemBackendContract class.
        /// </summary>
        public AggregatedOrderBookItemBackendContract(double? price = default(double?), double? volume = default(double?))
        {
            Price = price;
            Volume = volume;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "price")]
        public double? Price { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "volume")]
        public double? Volume { get; set; }

    }
}
