using Newtonsoft.Json;

namespace FarmFresh.Data.Models.EcontNomenclatures
{
    public class GeoLocation
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("confidence")]
        public int Confidence { get; set; }
    }
}
