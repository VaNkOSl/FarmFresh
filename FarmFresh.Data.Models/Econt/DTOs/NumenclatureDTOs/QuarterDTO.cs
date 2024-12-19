using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs
{
    public class QuarterDTO
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("cityID")]
        public int CityID { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("nameEn")]
        public string? NameEn { get; set; }
    }
}
