using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class ShipmentEditionResponseElementDTO
    {
        [JsonProperty("shipmentNum")]
        public int? ShipmentNum { get; set; }

        [JsonProperty("editionNum")]
        public int? EditionNum { get; set; }

        [JsonProperty("editionType")]
        public string? EditionType { get; set; }

        [JsonProperty("editionError")]
        public string? EditionError { get; set; }

        [JsonProperty("price")]
        public string? Price { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }
    }
}
