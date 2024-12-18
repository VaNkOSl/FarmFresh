using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs
{
    public class GeoLocationDTO
    {
        [JsonProperty("latitude")]
        public double? Latitude { get; set; }

        [JsonProperty("longitude")]
        public double? Longitude { get; set; }

        [JsonProperty("confidence")]
        public int? Confidence { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not GeoLocationDTO other) return false;
            return Latitude == other.Latitude
                && Longitude == other.Longitude
                && Confidence == other.Confidence;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude, Confidence);
        }
    }
}
