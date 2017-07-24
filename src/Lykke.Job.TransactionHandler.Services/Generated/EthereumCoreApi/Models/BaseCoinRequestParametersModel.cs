// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace LkeServices.Generated.EthereumCoreApi.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;

    public partial class BaseCoinRequestParametersModel
    {
        /// <summary>
        /// Initializes a new instance of the BaseCoinRequestParametersModel
        /// class.
        /// </summary>
        public BaseCoinRequestParametersModel()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the BaseCoinRequestParametersModel
        /// class.
        /// </summary>
        public BaseCoinRequestParametersModel(string coinAdapterAddress, string fromAddress, string toAddress, string amount)
        {
            CoinAdapterAddress = coinAdapterAddress;
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = amount;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "coinAdapterAddress")]
        public string CoinAdapterAddress { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "fromAddress")]
        public string FromAddress { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "toAddress")]
        public string ToAddress { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (CoinAdapterAddress == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "CoinAdapterAddress");
            }
            if (FromAddress == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "FromAddress");
            }
            if (ToAddress == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ToAddress");
            }
            if (Amount == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Amount");
            }
            if (Amount != null)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(Amount, "^[1-9][0-9]*$"))
                {
                    throw new ValidationException(ValidationRules.Pattern, "Amount", "^[1-9][0-9]*$");
                }
            }
        }
    }
}
