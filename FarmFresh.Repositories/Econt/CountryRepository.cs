using FarmFresh.Data;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories.Econt;
using System.Linq.Expressions;

namespace FarmFresh.Repositories.Econt
{
    public sealed class CountryRepository(FarmFreshDbContext data) : RepositoryBase<Country>(data), ICountryRepository
    {
        public async Task<Country> CreateCountryAsync(Country country)
        {
            await CreateAsync(country);
            return country;
        }

        public void DeleteCountry(Country country)
            => Delete(country);

        public void UpdateCountry(Country country)
            => Update(country);

        public async Task UpdateCountriesAsync(IEnumerable<Country> countries)
        {
            var existingCountries = _data.Countries.ToDictionary(c => c.Code2);
            var countriesToAdd = new List<Country>();

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
                    countriesToAdd.Add(country);
            }

            if(countriesToAdd.Count > 0)
                await _data.Countries.AddRangeAsync(countriesToAdd);

            _data.Countries.RemoveRange(existingCountries.Values);

            await _data.SaveChangesAsync();
        }

        public IQueryable<Country> FindAllCountries(bool trackChanges)
            => FindAll(trackChanges);

        public IQueryable<Country> FindCountriesByCondition(Expression<Func<Country, bool>> expression, bool trackChanges)
            => FindByCondition(expression, trackChanges);

        public Country? FindFirstCountryByCondition(Expression<Func<Country, bool>> expression, bool trackChanges)
            => FindFirstByCondition(expression, trackChanges);

        public async Task<Country?> FindFirstCountryByConditionAsync(Expression<Func<Country, bool>> expression, bool trackChanges)
            => await FindFirstByConditionAsync(expression, trackChanges);
    }
}