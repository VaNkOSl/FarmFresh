using FarmFresh.Data.Models.Econt.APIInterractionClasses;
using FarmFresh.Data.Models.Econt.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services.Contacts.Econt
{
    public interface IEcontNumenclaturesService
    {
        Task<List<CountryDTO>> GetCountries(GetCountriesRequest request);
        Task<List<CityDTO>> GetCities();
        Task<List<OfficeDTO>> GetOffices();
        Task<List<StreetDTO>> GetStreets();
        Task<List<QuarterDTO>> GetQuarters();
    }
}
