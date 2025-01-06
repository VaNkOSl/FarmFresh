using FarmFresh.Data.Models.Econt.Nomenclatures;
using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories.Econt;

public interface ICityRepository
{
    Task<City> CreateCityAsync(City city);
    void DeleteCity(City city);
    void UpdateCity(City city);
    Task UpdateCitiesAsync(IEnumerable<City> cities);
    IQueryable<City> FindAllCities(bool trackChanges);
    IQueryable<City> FindCitiesByCondition(Expression<Func<City, bool>> expression, bool trackChanges);
    City? FindFirstCityByCondition(Expression<Func<City, bool>> expression, bool trackChanges);
    Task<City?> FindFirstCityByConditionAsync(Expression<Func<City, bool>> expression, bool trackChanges);
}
