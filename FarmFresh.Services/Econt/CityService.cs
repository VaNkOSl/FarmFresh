using AutoMapper;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;

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
            string[] codes = ["bgr", "grc", "rou"];

            var tasks = codes.Select(code =>
                _econtNumenclaturesService.GetCitiesAsync(new GetCitiesRequest(code))
                .ContinueWith(task => _mapper.Map<List<City>>(task.Result))
            ).ToArray();

            var results = await Task.WhenAll(tasks);
            var cities = results.SelectMany(c => c).ToList();

            await _repositoryManager.CityRepository.UpdateCitiesAsync(cities);
        }

        public async Task<List<CityDTO>> GetCities()
        {
            var cities = await _econtNumenclaturesService.GetCitiesAsync(new GetCitiesRequest("bgr"));
            return cities;
        }

        public CityDTO? FindCityByName(string cityName)
        {
            var city = _repositoryManager.CityRepository
                .FindFirstCityByCondition(c => c.Name == cityName, false);

            if (city != null)
            {
                city.Country = _repositoryManager.CountryRepository
                    .FindFirstCountryByCondition(c => c.Id == city.CountryId, false);

                return MapToDTO(city, _mapper);
            }

            return null;
        }

        public async Task<CityDTO?> FindCityByNameAsync(string cityName)
        {
            var city = await _repositoryManager.CityRepository
            .FindFirstCityByConditionAsync(c => c.Name == cityName, false);

            if (city != null)
            {

                city.Country = _repositoryManager.CountryRepository
                    .FindFirstCountryByCondition(c => c.Id == city.CountryId, false);

                return MapToDTO(city, _mapper);
            }

            return null;
        }

        public CityDTO? FindCityByNameEn(string cityNameEn)
        {
            var city = _repositoryManager.CityRepository
            .FindFirstCityByCondition(c => c.NameEn == cityNameEn, false);

            if (city != null)
            {
                city.Country = _repositoryManager.CountryRepository
                    .FindFirstCountryByCondition(c => c.Id == city.CountryId, false);

                return MapToDTO(city, _mapper);
            }

            return null;
        }

        public async Task<CityDTO?> FindCityByNameEnAsync(string cityNameEn)
        {
            var city = await _repositoryManager.CityRepository
                .FindFirstCityByConditionAsync(c => c.NameEn == cityNameEn, false);

            if (city != null)
            {
                city.Country = _repositoryManager.CountryRepository
                    .FindFirstCountryByCondition(c => c.Id == city.CountryId, false);

                return MapToDTO(city, _mapper);
            }

            return null;
        }

        private static CityDTO MapToDTO(City city, IMapper mapper) => mapper.Map<CityDTO>(city);
    }
}
