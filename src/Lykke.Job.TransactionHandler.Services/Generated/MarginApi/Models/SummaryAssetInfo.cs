// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Job.TransactionHandler.Services.Generated.MarginApi.Models
{
    public partial class SummaryAssetInfo
    {
        /// <summary>
        /// Initializes a new instance of the SummaryAssetInfo class.
        /// </summary>
        public SummaryAssetInfo() { }

        /// <summary>
        /// Initializes a new instance of the SummaryAssetInfo class.
        /// </summary>
        public SummaryAssetInfo(string assetPairId = default(string), double? volumeLong = default(double?), double? volumeShort = default(double?), double? pnL = default(double?))
        {
            AssetPairId = assetPairId;
            VolumeLong = volumeLong;
            VolumeShort = volumeShort;
            PnL = pnL;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "assetPairId")]
        public string AssetPairId { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "volumeLong")]
        public double? VolumeLong { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "volumeShort")]
        public double? VolumeShort { get; set; }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "pnL")]
        public double? PnL { get; set; }

    }
}
