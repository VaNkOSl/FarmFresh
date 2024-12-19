using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class PreviousShipmentDTO
    {
        [JsonProperty("shipmentNumber")]
        public int? ShipmentNumber { get; set; }

        [JsonProperty("reason")]
        public string? Reason { get; set; }

        [JsonProperty("pdfURL")]
        public string? PDFURL { get; set; }
    }
}
