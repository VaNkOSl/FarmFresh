using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetShipmentStatusesRequest(List<string> shipmentNumbers) : RequestBase
    {
        public List<string>? ShipmentNumbers { get; set; } = shipmentNumbers;
    }
}
