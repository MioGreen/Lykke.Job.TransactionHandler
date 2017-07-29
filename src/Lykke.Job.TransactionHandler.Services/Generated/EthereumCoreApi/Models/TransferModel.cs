// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

using Microsoft.Rest;
using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Services.Generated.EthereumCoreApi.Models
{
    public partial class TransferModel
    {
        /// <summary>
        /// Initializes a new instance of the TransferModel class.
        /// </summary>
        public TransferModel()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TransferModel class.
        /// </summary>
        public TransferModel(System.Guid id, string coinAdapterAddress, string fromAddress, string toAddress, string amount, string sign = default(string))
        {
            Sign = sign;
            Id = id;
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
        [JsonProperty(PropertyName = "sign")]
        public string Sign { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public System.Guid Id { get; set; }

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
