using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs
{
    public class ServingOfficeElementDTO
    {
        [JsonProperty("officeCode")]
        public string? OfficeCode { get; set; }

        [JsonProperty("servingType")]
        public string? ServingType { get; set; }
    }
}
