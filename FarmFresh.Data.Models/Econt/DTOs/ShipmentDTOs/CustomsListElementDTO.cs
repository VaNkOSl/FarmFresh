using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class CustomsListElementDTO
    {
        [JsonProperty("cn")]
        public string? CN { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("sum")]
        public double? Sum { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }
    }
}
