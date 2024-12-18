using FarmFresh.Data;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories.Econt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

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

            var citiesToAdd = new List<City>();

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

                    var newOffices = city.ServingOffices ?? new List<ServingOfficeElement>();
                    var existingOffices = existingCity.ServingOffices ?? new List<ServingOfficeElement>();

                    var officesToKeep = existingOffices
                        .Where(eo => newOffices.Any(no =>
                            no.OfficeCode == eo.OfficeCode))
                        .ToList();

                    var officesToAdd = newOffices
                        .Where(no => !existingOffices.Any(eo =>
                            eo.OfficeCode == no.OfficeCode))
                        .Select(no => new ServingOfficeElement { OfficeCode = no.OfficeCode, ServingType = no.ServingType})
                        .ToList();

                    var updatedOffices = officesToKeep.Concat(officesToAdd).ToList();

                    existingCity.ServingOffices = updatedOffices;
                }
                else
                    citiesToAdd.Add(city);
            }

            await _data.Cities.AddRangeAsync(citiesToAdd);

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
