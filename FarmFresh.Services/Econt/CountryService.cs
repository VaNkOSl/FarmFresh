using AutoMapper;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories.Econt;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services.Econt
{
    public class CountryService(
        IEcontNumenclaturesService econtNumenclaturesService,
        IRepositoryManager repositoryManager,
        IMapper mapper)
        : ICountryService
    {
        private IEcontNumenclaturesService _econtNumenclaturesService = econtNumenclaturesService;
        private IRepositoryManager _repositoryManager = repositoryManager;
        private IMapper _mapper = mapper;

        public async Task UpdateCountriesAsync()
        {
            var countriesDTOs = await _econtNumenclaturesService.GetCountriesAsync(new GetCountriesRequest());
            var countries = _mapper.Map<List<Country>>(countriesDTOs);
            await _repositoryManager.CountryRepository.UpdateCountriesAsync(countries);
        }
    }
}
