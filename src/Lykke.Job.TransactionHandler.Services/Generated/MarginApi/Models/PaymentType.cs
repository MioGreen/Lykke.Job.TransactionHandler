// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{

    /// <summary>
    /// Defines values for PaymentType.
    /// </summary>
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum PaymentType
    {
        [System.Runtime.Serialization.EnumMember(Value = "Transfer")]
        Transfer,
        [System.Runtime.Serialization.EnumMember(Value = "Swift")]
        Swift
    }
}
