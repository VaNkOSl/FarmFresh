using AutoMapper;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services.Econt
{
    public class CityService(
        IEcontNumenclaturesService econtNumenclaturesService,
        IRepositoryManager repositoryManager,
        IMapper mapper)
        : ICityService
    {
        private readonly IEcontNumenclaturesService _econtNumenclaturesService = econtNumenclaturesService;
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public async Task UpdateCitiesAsync()
        {
            string[] codes = { "bgr", "grc", "rou" };
            List<City> cities = new();

            foreach (var code in codes)
            {
                GetCitiesRequest request = new();
                request.CountryCode = code;

                var citiesDTOs = await _econtNumenclaturesService.GetCitiesAsync(request);
                var citiesMapped = _mapper.Map<List<City>>(citiesDTOs);

                cities.AddRange(citiesMapped);
            }

            await _repositoryManager.CityRepository.UpdateCitiesAsync(cities);
        }
    }
}
