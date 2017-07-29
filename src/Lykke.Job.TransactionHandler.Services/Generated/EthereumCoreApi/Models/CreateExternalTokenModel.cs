// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

using Microsoft.Rest;
using Newtonsoft.Json;

namespace Lykke.Job.TransactionHandler.Services.Generated.EthereumCoreApi.Models
{
    public partial class CreateExternalTokenModel
    {
        /// <summary>
        /// Initializes a new instance of the CreateExternalTokenModel class.
        /// </summary>
        public CreateExternalTokenModel()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CreateExternalTokenModel class.
        /// </summary>
        public CreateExternalTokenModel(string tokenName, bool allowEmission, string tokenSymbol, string version, byte[] divisibility, string initialSupply = default(string))
        {
            TokenName = tokenName;
            AllowEmission = allowEmission;
            TokenSymbol = tokenSymbol;
            Version = version;
            InitialSupply = initialSupply;
            Divisibility = divisibility;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "tokenName")]
        public string TokenName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "allowEmission")]
        public bool AllowEmission { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "tokenSymbol")]
        public string TokenSymbol { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "initialSupply")]
        public string InitialSupply { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "divisibility")]
        public byte[] Divisibility { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (TokenName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "TokenName");
            }
            if (TokenSymbol == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "TokenSymbol");
            }
            if (Version == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Version");
            }
            if (Divisibility == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Divisibility");
            }
            if (InitialSupply != null)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(InitialSupply, "^([1-9][0-9]*)|([0])$"))
                {
                    throw new ValidationException(ValidationRules.Pattern, "InitialSupply", "^([1-9][0-9]*)|([0])$");
                }
            }
        }
    }
}
