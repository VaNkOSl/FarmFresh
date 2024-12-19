using FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class DeleteLabelsResponse
    {
        [JsonProperty("results")]
        public List<DeleteLabelsResultElementDTO>? Results { get; set; }
    }
}
