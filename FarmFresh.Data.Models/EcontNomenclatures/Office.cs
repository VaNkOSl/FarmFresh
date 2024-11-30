using FarmFresh.Data.Models.CustomConverters;
using FarmFresh.Data.Models.Enums;
using Newtonsoft.Json;
using System.Numerics;

namespace FarmFresh.Data.Models.EcontNomenclatures
{
    public class Office
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("isMPS")]
        public bool? IsMPS { get; set; }

        [JsonProperty("isAPS")]
        public bool? IsAPS { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("nameEn")]
        public string? NameEn { get; set; }

        [JsonProperty("phones")]
        public List<string>? Phones { get; set; }

        [JsonProperty("emails")]
        public List<string>? Emails { get; set; }

        [JsonProperty("address")]
        public Address? Address { get; set; }

        [JsonProperty("info")]
        public string? Info { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("language")]
        public string? Language { get; set; }

        [JsonProperty("normalBusinessHoursFrom")]
        public TimeOnly NormalBusinessHoursFrom { get; set; }

        [JsonProperty("normalBusinessHoursTo")]
        public TimeOnly NormalBusinessHoursTo { get; set; }

        [JsonProperty("halfDayBusinessHoursFrom")]
        public TimeOnly HalfDayBusinessHoursFrom { get; set; }

        [JsonProperty("halfDayBusinessHoursTo")]
        public TimeOnly HalfDayBusinessHoursTo { get; set; }

        [JsonProperty("sundayBusinessHoursFrom")]
        public TimeOnly SundayBusinessHoursFrom { get; set; }

        [JsonProperty("sundayBusinessHoursTo")]
        public TimeOnly SundayBusinessHoursTo { get; set; }

        [JsonProperty("shipmentType")]
        [JsonConverter(typeof(ShipmentTypeConverter))]
        public ShipmentType? ShipmentType { get; set; }

        [JsonProperty("partnerCode")]
        public string? PartnerCode { get; set; }

        [JsonProperty("hubCode")]
        public string? HubCode { get; set; }

        [JsonProperty("hubName")]
        public string? HubName { get; set; }

        [JsonProperty("hubNameEn")]
        public string? HubNameEn { get; set; }

        [JsonProperty("isDrive")]
        public bool? IsDrive { get; set; }
    }
}
