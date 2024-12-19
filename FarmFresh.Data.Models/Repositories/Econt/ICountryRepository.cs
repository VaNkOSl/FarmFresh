using FarmFresh.Data.Models.Econt.Nomenclatures;
using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories.Econt
{
    public interface ICountryRepository
    {
        Task<Country> CreateCountryAsync(Country country);
        void DeleteCountry(Country country);
        void UpdateCountry(Country country);
        Task UpdateCountriesAsync(IEnumerable<Country> countries);
        IQueryable<Country> FindAllCountries(bool trackChanges);
        IQueryable<Country> FindCountriesByCondition(Expression<Func<Country, bool>> expression, bool trackChanges);
        Country? FindFirstCountryByCondition(Expression<Func<Country, bool>> expression, bool trackChanges);
        Task<Country?> FindFirstCountryByConditionAsync(Expression<Func<Country, bool>> expression, bool trackChanges);
    }
}
