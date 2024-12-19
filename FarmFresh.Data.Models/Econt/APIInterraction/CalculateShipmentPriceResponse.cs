using FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs;
using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class CalculateShipmentPriceResponse : ResponseBase
    {
        [JsonProperty("label")]
        public ShipmentStatusDTO? Label { get; set; }
    }
}
