using FarmFresh.Data.Models.Econt.Nomenclatures;
using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories.Econt
{
    public interface IQuarterRepository
    {
        Task<Quarter> CreateQuarterAsync(Quarter quarter);
        Task<Quarter?> GetQuarterByIdAsync(int id);
        void DeleteQuarter(Quarter quarter);
        void UpdateQuarter(Quarter quarter);
        Task UpdateQuartersAsync(IEnumerable<Quarter> quarters);
        IQueryable<Quarter> FindAllQuarters(bool trackChanges);
        IQueryable<Quarter> FindQuartersByCondition(Expression<Func<Quarter, bool>> expression, bool trackChanges);
        Quarter? FindFirstQuarterByCondition(Expression<Func<Quarter, bool>> expression, bool trackChanges);
        Task<Quarter?> FindFirstQuarterByConditionAsync(Expression<Func<Quarter, bool>> expression, bool trackChanges);
    }
}
