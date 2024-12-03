using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services.Contacts.Econt.APIServices
{
    public interface IEcontNumenclaturesService
    {
        Task<List<CountryDTO>> GetCountriesAsync(GetCountriesRequest request);
        Task<List<CityDTO>> GetCitiesAsync();
        Task<List<OfficeDTO>> GetOfficesAsync();
        Task<List<StreetDTO>> GetStreetsAsync();
        Task<List<QuarterDTO>> GetQuartersAsync();
    }
}
