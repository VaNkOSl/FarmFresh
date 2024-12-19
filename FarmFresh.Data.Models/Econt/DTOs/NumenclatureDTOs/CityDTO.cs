using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs
{
    public class CityDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("country")]
        public CountryDTO? Country { get; set; }

        [JsonProperty("postCode")]
        public string? PostCode { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("nameEn")]
        public string? NameEn { get; set; }

        [JsonProperty("regionName")]
        public string? RegionName { get; set; }

        [JsonProperty("regionNameEn")]
        public string? RegionNameEn { get; set; }

        [JsonProperty("phoneCode")]
        public string? PhoneCode { get; set; }

        [JsonProperty("location")]
        public GeoLocationDTO? Location { get; set; }

        [JsonProperty("expressCityDeliveries")]
        public bool? ExpressCityDeliveries { get; set; }

        [JsonProperty("monday")]
        public bool? Monday { get; set; }

        [JsonProperty("tuesday")]
        public bool? Tuesday { get; set; }

        [JsonProperty("wednesday")]
        public bool? Wednesday { get; set; }

        [JsonProperty("thursday")]
        public bool? Thursday { get; set; }

        [JsonProperty("friday")]
        public bool? Friday { get; set; }

        [JsonProperty("saturday")]
        public bool? Saturday { get; set; }

        [JsonProperty("sunday")]
        public bool? Sunday { get; set; }

        [JsonProperty("serviceDays")]
        public int? ServiceDays { get; set; }

        [JsonProperty("zoneId")]
        public int? ZoneId { get; set; }

        [JsonProperty("zoneName")]
        public string? ZoneName { get; set; }

        [JsonProperty("zoneNameEn")]
        public string? ZoneNameEn { get; set; }

        [JsonProperty("servingOffices")]
        public List<ServingOfficeElementDTO>? ServingOffices { get; set; }

        public CityDTO() { }

        public CityDTO(
            CountryDTO country,
            string cityName,
            string cityPostCode)
        {
            Country = country;
            Name = cityName;
            PostCode = cityPostCode;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not CityDTO other) return false;
            return Id == other.Id
                && Country == other.Country
                && Name == other.Name
                && NameEn == other.NameEn
                && RegionName == other.RegionName
                && RegionNameEn == other.RegionNameEn
                && PhoneCode == other.PhoneCode
                && Location == other.Location
                && ZoneId == other.ZoneId
                && ZoneName == other.ZoneName
                && ZoneNameEn == other.ZoneNameEn;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
