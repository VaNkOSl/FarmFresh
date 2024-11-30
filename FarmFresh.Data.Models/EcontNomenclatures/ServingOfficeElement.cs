using Newtonsoft.Json;

namespace FarmFresh.Data.Models.EcontNomenclatures
{
    public class ServingOfficeElement
    {
        [JsonProperty("officeCode")]
        public string? OfficeCode { get; set; }

        [JsonProperty("servingType")]
        public string? ServingType { get; set; }
    }
}
