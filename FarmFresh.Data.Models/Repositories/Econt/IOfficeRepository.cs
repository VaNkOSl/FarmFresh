using FarmFresh.Data.Models.Econt.Nomenclatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Repositories.Econt
{
    public interface IOfficeRepository
    {
        Task<Office> CreateOfficeAsync(Office office);
        Task<Office?> GetOfficeByIdAsync(int id);
        void DeleteOffice(Office office);
        void UpdateOffice(Office office);
        Task UpdateOfficesAsync(IEnumerable<Office> offices);
        IQueryable<Office> FindAllOffices(bool trackChanges);
        IQueryable<Office> FindOfficesByCondition(Expression<Func<Office, bool>> expression, bool trackChanges);
        Office? FindFirstOfficeByCondition(Expression<Func<Office, bool>> expression, bool trackChanges);
        Task<Office?> FindFirstOfficeByConditionAsync(Expression<Func<Office, bool>> expression, bool trackChanges);
    }
}
