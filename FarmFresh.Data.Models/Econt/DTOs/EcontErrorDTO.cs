using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Econt.DTOs
{
    public class EcontErrorDTO
    {
        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("fields")]
        public string? Fields { get; set; }

        [JsonProperty("innerErrors")]
        public List<EcontErrorDTO>? InnerErrors { get; set; }
    }
}
