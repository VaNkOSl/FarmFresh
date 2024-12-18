using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class PackingListElementDTO
    {
        [JsonProperty("inventoryNum")]
        public string? InventoryNum { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("weight")]
        public double? Weight { get; set; }

        [JsonProperty("count")]
        public int? Count { get; set; }

        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("file")]
        public HostedFileDTO? HostedFile { get; set; }

        [JsonProperty("alternativeDepartment")]
        public string? AlternativeDepartment { get; set; }
    }
}
