using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class PackElementDTO
    {
        [JsonProperty("width")]
        public float? Width { get; set; }

        [JsonProperty("height")]
        public float? Height { get; set; }

        [JsonProperty("length")]
        public float? Length { get; set; }

        [JsonProperty("weight")]
        public float? Weight { get; set; }
    }
}
