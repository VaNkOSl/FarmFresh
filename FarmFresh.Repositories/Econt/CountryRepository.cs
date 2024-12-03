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
    internal sealed class CountryRepository(FarmFreshDbContext data) : RepositoryBase<Country>(data), ICountryRepository
    {
        public async Task<Country> CreateCountryAsync(Country country)
        {
            await CreateAsync(country);
            return country;
        }

        public void DeleteCountry(Country country) => Delete(country);

        public void UpdateCountry(Country country) => Update(country);

        public async Task UpdateCountriesAsync(IEnumerable<Country> countries)
        {
            _data.Countries.RemoveRange(_data.Countries);
            _data.Countries.AddRange(countries);
            await _data.SaveChangesAsync();
        }

        public IQueryable<Country> FindAllCountries(bool trackChanges) => FindAll(trackChanges);

        public IQueryable<Country> FindCountryByConditionAsync(Expression<Func<Country, bool>> expression, bool trackChanges)
            => FindByCondition(expression, trackChanges);

        public Task<Country?> GetCountryByIdAsync(int id) => GetByIdAsync(id);
    }
}