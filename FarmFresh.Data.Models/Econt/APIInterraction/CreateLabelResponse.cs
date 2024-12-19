using FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs;
using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class CreateLabelResponse : ResponseBase
    {
        [JsonProperty("label")]
        public ShipmentStatusDTO? Label { get; set; }

        [JsonProperty("blockingPaymentURL")]
        public string? BlockingPaymentURL { get; set; }

        [JsonProperty("courierRequestID")]
        public int? CourierRequestID { get; set; }

        [JsonProperty("payAfterAcceptIgnored")]
        public string? PayAfterAcceptIgnored { get; set; }
    }
}
