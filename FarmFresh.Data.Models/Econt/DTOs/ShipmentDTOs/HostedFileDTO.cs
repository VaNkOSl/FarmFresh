using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class HostedFileDTO
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("filename")]
        public string? Filename { get; set; }

        [JsonProperty("mimeType")]
        public string? MimeType { get; set; }
        
        public string? URL { get; set; }

        [JsonProperty("content")]
        public byte[]? Content { get; set; }
    }
}
