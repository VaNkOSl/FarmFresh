﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.Data.Models.Econt.DTOs
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
    }
}
