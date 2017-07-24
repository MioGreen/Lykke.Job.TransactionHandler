// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace LkeServices.Generated.ChronoBankApi.Models
{
    public partial class ApiError
    {
        /// <summary>
        /// Initializes a new instance of the ApiError class.
        /// </summary>
        public ApiError() { }

        /// <summary>
        /// Initializes a new instance of the ApiError class.
        /// </summary>
        /// <param name="code">Possible values include: 'Exception',
        /// 'ContractPoolEmpty', 'MissingRequiredParams'</param>
        public ApiError(string code = default(string), string message = default(string))
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// Gets or sets possible values include: 'Exception',
        /// 'ContractPoolEmpty', 'MissingRequiredParams'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

    }
}
