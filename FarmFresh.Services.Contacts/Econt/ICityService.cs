using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;

namespace FarmFresh.Services.Contacts.Econt
{
    public interface ICityService
    {
        Task UpdateCitiesAsync();

        CityDTO? FindCityByName(string cityName);
        Task<CityDTO?> FindCityByNameAsync(string cityName);

        CityDTO? FindCityByNameEn(string cityNameEn);
        Task<CityDTO?> FindCityByNameEnAsync(string cityNameEn);

        public Task<List<CityDTO>> GetCities();
    }
}
