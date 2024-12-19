using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs
{
    public class CountryDTO
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("code2")]
        public string? Code2 { get; set; }

        [JsonProperty("code3")]
        public string? Code3 { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("nameEn")]
        public string? NameEn { get; set; }

        [JsonProperty("isEU")]
        public bool? IsEU { get; set; }

        public CountryDTO() { }

        public CountryDTO(
            string code3)
        {
            Code3 = code3;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not CountryDTO other) return false;
            return Code2 == other.Code2
                && Code3 == other.Code3
                && Name == other.Name
                && NameEn == other.NameEn
                && IsEU == other.IsEU;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
