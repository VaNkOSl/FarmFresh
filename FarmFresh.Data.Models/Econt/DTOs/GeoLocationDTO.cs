using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs
{
    public class GeoLocationDTO
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("confidence")]
        public int Confidence { get; set; }
    }
}
