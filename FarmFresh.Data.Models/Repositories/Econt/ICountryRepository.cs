using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Repositories.Econt
{
    public interface ICountryRepository
    {
        Task<Country> CreateCountryAsync(Country country);
        Task<Country?> GetCountryByIdAsync(int id);
        void DeleteCountry(Country country);
        void UpdateCountry(Country country);
        Task UpdateCountriesAsync(IEnumerable<Country> countries);
        IQueryable<Country> FindAllCountries(bool trackChanges);
        IQueryable<Country> FindCountryByConditionAsync(Expression<Func<Country, bool>> expression, bool trackChanges);
    }
}
