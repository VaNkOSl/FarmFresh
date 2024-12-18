using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class NextShipmentElementDTO
    {
        [JsonProperty("shipmentNumber")]
        public string? ShipmentNumber { get; set; }

        [JsonProperty("reason")]
        public string? Reason { get; set; }

        [JsonProperty("pdfURL")]
        public string? PDFURL { get; set; }
    }
}
