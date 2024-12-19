using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class DeleteLabelsResultElementDTO
    {
        [JsonProperty("shipmentNum")]
        public string? ShipmentNum { get; set; }

        [JsonProperty("error")]
        public EcontErrorDTO? Error { get; set; }
    }
}
