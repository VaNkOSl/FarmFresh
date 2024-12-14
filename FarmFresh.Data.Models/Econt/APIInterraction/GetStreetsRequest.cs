using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetStreetsRequest : RequestBase
    {
        public int CityID { get; set; }

        public GetStreetsRequest() { }
        public GetStreetsRequest(int cityID) => CityID = cityID;
    }
}
