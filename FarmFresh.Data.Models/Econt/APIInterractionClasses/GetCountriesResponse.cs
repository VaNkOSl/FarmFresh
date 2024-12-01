using FarmFresh.Data.Models.Econt.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Econt.APIInterractionClasses
{
    public class GetCountriesResponse
    {
        public List<CountryDTO>? Countries { get; set; }
    }
}
