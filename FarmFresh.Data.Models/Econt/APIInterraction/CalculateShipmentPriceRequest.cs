using FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs;
using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class CalculateShipmentPriceRequest(ShippingLabelDTO label) : RequestBase
    {
        [JsonProperty("label")]
        public ShippingLabelDTO? Label { get; set; } = label;

        [JsonProperty("mode")]
        public string? Mode = "calculate";
    }
}
