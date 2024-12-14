using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetQuartersRequest : RequestBase
    {
        public int CityID { get; set; }

        public GetQuartersRequest() { }
        public GetQuartersRequest(int cityID) => CityID = cityID;
    }
}
