using Newtonsoft.Json;

namespace FarmFresh.Data.Models.EcontNomenclatures
{
    public class Address
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("city")]
        public City? City { get; set; }

        [JsonProperty("fullAddress")]
        public string? FullAddress { get; set; }

        [JsonProperty("fullAddressEn")]
        public string? FullAddressEn { get; set; }

        [JsonProperty("quarter")]
        public string? Quarter { get; set; }

        [JsonProperty("street")]
        public string? Street { get; set; }

        [JsonProperty("num")]
        public string? Num { get; set; }

        [JsonProperty("other")]
        public string? Other { get; set; }

        [JsonProperty("location")]
        public GeoLocation? Location { get; set; }

        [JsonProperty("zip")]
        public string? Zip { get; set; }

        [JsonProperty("hezid")]
        public string? Hezid { get; set; }
    }
}
