// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace LkeServices.Generated.MarginApi.Models
{
    public partial class MtBackendResponseBoolean
    {
        /// <summary>
        /// Initializes a new instance of the MtBackendResponseBoolean class.
        /// </summary>
        public MtBackendResponseBoolean() { }

        /// <summary>
        /// Initializes a new instance of the MtBackendResponseBoolean class.
        /// </summary>
        public MtBackendResponseBoolean(bool? result = default(bool?), string message = default(string))
        {
            Result = result;
            Message = message;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "result")]
        public bool? Result { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

    }
}
