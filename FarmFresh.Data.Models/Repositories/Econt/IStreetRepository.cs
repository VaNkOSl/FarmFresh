using FarmFresh.Data.Models.Econt.Nomenclatures;
using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories.Econt
{
    public interface IStreetRepository
    {
        Task<Street> CreateStreetAsync(Street street);
        Task<Street?> GetStreetByIdAsync(int id);
        void DeleteStreet(Street street);
        void UpdateStreet(Street street);
        Task UpdateStreetsAsync(IEnumerable<Street> streets);
        IQueryable<Street> FindAllStreets(bool trackChanges);
        IQueryable<Street> FindStreetsByCondition(Expression<Func<Street, bool>> expression, bool trackChanges);
        Street? FindFirstStreetByCondition(Expression<Func<Street, bool>> expression, bool trackChanges);
        Task<Street?> FindFirstStreetByConditionAsync(Expression<Func<Street, bool>> expression, bool trackChanges);
    }
}
