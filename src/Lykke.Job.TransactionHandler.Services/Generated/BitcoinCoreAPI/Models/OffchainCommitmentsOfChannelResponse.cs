// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.BitcoinCoreAPI.Models
{
    public partial class OffchainCommitmentsOfChannelResponse
    {
        /// <summary>
        /// Initializes a new instance of the
        /// OffchainCommitmentsOfChannelResponse class.
        /// </summary>
        public OffchainCommitmentsOfChannelResponse() { }

        /// <summary>
        /// Initializes a new instance of the
        /// OffchainCommitmentsOfChannelResponse class.
        /// </summary>
        public OffchainCommitmentsOfChannelResponse(System.Collections.Generic.IList<OffchainCommitmentInfo> commitments = default(System.Collections.Generic.IList<OffchainCommitmentInfo>))
        {
            Commitments = commitments;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "commitments")]
        public System.Collections.Generic.IList<OffchainCommitmentInfo> Commitments { get; set; }

    }
}
