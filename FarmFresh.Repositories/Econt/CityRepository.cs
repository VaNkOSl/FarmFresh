using FarmFresh.Data;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories.Econt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        
        public void DeleteCity(City city)
            => Delete(city);
        
        public void UpdateCity(City city)
            => Update(city);
        
        public async Task UpdateCitiesAsync(IEnumerable<City> cities)
        {
            if(cities.IsNullOrEmpty()) return;

            var existingCountries = _data.Countries.ToDictionary(c => c.Code2);

            foreach (var city in cities)
            {
                var country = existingCountries[city.Country!.Code2];

                if(country == null) continue;

                city.CountryId = country.Id;
                city.Country = null;

                var existingCity = await _data.Cities
                    .Where(c => c.Id == city.Id)
                    .Include(c => c.ServingOffices)
                    .FirstOrDefaultAsync();
                
                if(existingCity != null)
                {
                    _data.Entry(existingCity).CurrentValues.SetValues(city);

                    var officesToAdd = city.ServingOffices!
                        .Where(nso => !existingCity.ServingOffices!.Any(eo => eo.OfficeCode == nso.OfficeCode))
                        .ToList();

                    var officesToRemove = existingCity.ServingOffices!
                        .Where(eo => !city.ServingOffices!.Any(nso => nso.OfficeCode == eo.OfficeCode))
                        .ToList();

                    _data.AddRange(officesToAdd);
                    _data.RemoveRange(officesToRemove);
                }
                else
                    _data.Cities.Add(city);
            }

            await _data.SaveChangesAsync();
        }

        public IQueryable<City> FindAllCities(bool trackChanges)
            => FindAll(trackChanges);

        public IQueryable<City> FindCitiesByCondition(Expression<Func<City, bool>> expression, bool trackChanges)
            => FindByCondition(expression, trackChanges);

        public async Task<City?> GetCityByIdAsync(int id) => await GetByIdAsync(id);

        public City? FindFirstCityByCondition(Expression<Func<City, bool>> expression, bool trackChanges)
            => FindFirstByCondition(expression, trackChanges);

        public async Task<City?> FindFirstCityByConditionAsync(Expression<Func<City, bool>> expression, bool trackChanges)
            => await FindFirstByConditionAsync(expression, trackChanges);
    }
}
