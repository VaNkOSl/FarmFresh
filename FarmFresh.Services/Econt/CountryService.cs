using AutoMapper;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;

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

        private static CountryDTO MapToDTO(Country country, IMapper mapper) => mapper.Map<CountryDTO>(country);
    }
}
