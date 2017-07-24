// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace LkeServices.Generated.EthereumCoreApi.Models
{
    using Newtonsoft.Json;

    public partial class CheckSignResponse
    {
        /// <summary>
        /// Initializes a new instance of the CheckSignResponse class.
        /// </summary>
        public CheckSignResponse()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CheckSignResponse class.
        /// </summary>
        public CheckSignResponse(bool? signIsCorrect = default(bool?))
        {
            SignIsCorrect = signIsCorrect;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "signIsCorrect")]
        public bool? SignIsCorrect { get; set; }

    }
}
