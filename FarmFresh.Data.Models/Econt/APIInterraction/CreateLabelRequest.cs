using FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs;
using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class CreateLabelRequest: RequestBase
    {
        [JsonProperty("label")]
        public ShippingLabelDTO? Label { get; set; }

        [JsonProperty("requestCourierTimeFrom")]
        public int? RequestCourierTimeFrom { get; set; }

        [JsonProperty("requestCourierTimeTo")]
        public int? RequestCourierTimeTo { get; set; }

        [JsonProperty("mode")]
        public string? Mode { get; set; }

        public CreateLabelRequest(
            ShippingLabelDTO label,
        int requestCourierTimeFrom,
        int requestCourierTimeTo)
        {
            Label = label;
            RequestCourierTimeFrom = requestCourierTimeFrom;
            RequestCourierTimeTo = requestCourierTimeTo;
            Mode = "create";
        }

        public CreateLabelRequest(ShippingLabelDTO label)
        {
            Label = label;
            Mode = "create";
        }
    }
}
