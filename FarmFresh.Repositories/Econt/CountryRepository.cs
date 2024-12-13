using FarmFresh.Data;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories.Econt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Repositories.Econt
{
    public sealed class CountryRepository(FarmFreshDbContext data) : RepositoryBase<Country>(data), ICountryRepository
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
            var existingCountries = _data.Countries.ToDictionary(c => c.Code2);

            foreach (var country in countries)
            {
                if (existingCountries.TryGetValue(country.Code2, out var existingCountry))
                {
                    existingCountry.Name = country.Name;
                    existingCountry.NameEn = country.NameEn;
                    existingCountry.IsEU = country.IsEU;
                    existingCountries.Remove(country.Code2);
                }
                else
                    _data.Countries.Add(country);
            }

            _data.Countries.RemoveRange(existingCountries.Values);

            await _data.SaveChangesAsync();
        }

        public IQueryable<Country> FindAllCountries(bool trackChanges) => FindAll(trackChanges);

        public IQueryable<Country> FindCountriesByCondition(Expression<Func<Country, bool>> expression, bool trackChanges)
            => FindByCondition(expression, trackChanges);

        public async Task<Country?> GetCountryByIdAsync(int id) => await GetByIdAsync(id);

        public Country? FindFirstCountryByCondition(Expression<Func<Country, bool>> expression, bool trackChanges)
            => FindFirstByCondition(expression, trackChanges);

        public Task<Country?> FindFirstCountryByConditionAsync(Expression<Func<Country, bool>> expression, bool trackChanges)
            => FindFirstByConditionAsync(expression, trackChanges);
    }
}