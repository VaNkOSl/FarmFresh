using FarmFresh.Data.Models.Econt.Nomenclatures;
using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories.Econt
{
    public interface IOfficeRepository
    {
        Task<Office> CreateOfficeAsync(Office office);
        void DeleteOffice(Office office);
        void UpdateOffice(Office office);
        Task UpdateOfficesAsync(IEnumerable<Office> offices);
        IQueryable<Office> FindAllOffices(bool trackChanges);
        IQueryable<Office> FindOfficesByCondition(Expression<Func<Office, bool>> expression, bool trackChanges);
        Office? FindFirstOfficeByCondition(Expression<Func<Office, bool>> expression, bool trackChanges);
        Task<Office?> FindFirstOfficeByConditionAsync(Expression<Func<Office, bool>> expression, bool trackChanges);
    }
}
