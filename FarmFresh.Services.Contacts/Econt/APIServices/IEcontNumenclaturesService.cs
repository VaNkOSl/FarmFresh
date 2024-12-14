﻿using FarmFresh.Data.Models.Econt.APIInterraction;
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
        Task<List<CityDTO>> GetCitiesAsync(GetCitiesRequest request);
        Task<List<OfficeDTO>> GetOfficesAsync(GetOfficesRequest request);
        Task<List<StreetDTO>> GetStreetsAsync(GetStreetsRequest request);
        Task<List<QuarterDTO>> GetQuartersAsync(GetQuartersRequest request);
    }
}
