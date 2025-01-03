using FarmFresh.Data.Models;
using System.Linq.Expressions;

namespace FarmFresh.Repositories.Contacts;

public interface IUserRepository
{
    Task<ApplicationUser> CreateUserAsync(ApplicationUser user);

    void DeleteUser(ApplicationUser user);

    void UpdateUser(ApplicationUser user);

    IQueryable<ApplicationUser> FindUsersByConditionAsync(Expression<Func<ApplicationUser, bool>> expression, bool trackChanges);

    IQueryable<ApplicationUser> GetAllUsers(bool trackChanges);
}
