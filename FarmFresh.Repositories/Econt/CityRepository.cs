using FarmFresh.Data;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories.Econt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Repositories.Econt
{
    public class CityRepository(FarmFreshDbContext data) : RepositoryBase<City>(data), ICityRepository
    {
        public async Task<City> CreateCityAsync(City city)
        {
            await CreateAsync(city);
            return city;
        }
        
        public void DeleteCity(City city) => Delete(city);
        
        public void UpdateCity(City city) => Update(city);
        
        public async Task UpdateCitiesAsync(IEnumerable<City> cities)
        {
            _data.Cities.RemoveRange(_data.Cities);
            _data.Cities.AddRange(cities);
            await _data.SaveChangesAsync();
        }

        public IQueryable<City> FindAllCities(bool trackChanges) => FindAll(trackChanges);

        public IQueryable<City> FindCityByConditionAsync(Expression<Func<City, bool>> expression, bool trackChanges)
            => FindByCondition(expression, trackChanges);

        public async Task<City?> GetCityByIdAsync(int id) => await GetByIdAsync(id);
    }
}
