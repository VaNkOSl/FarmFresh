﻿using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs
{
    public class AddressDTO
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("city")]
        public CityDTO? City { get; set; }

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
        public GeoLocationDTO? Location { get; set; }

        [JsonProperty("zip")]
        public string? Zip { get; set; }

        [JsonProperty("hezid")]
        public string? Hezid { get; set; }
    }
}
