using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class DeleteLabelsRequest(List<string> shipemtNumbers) : RequestBase
    {
        [JsonProperty("shipmentNumbers")]
        public List<string>? ShipmentNumbers { get; set; } = shipemtNumbers;
    }
}
