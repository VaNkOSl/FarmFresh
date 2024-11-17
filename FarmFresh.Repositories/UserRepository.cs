using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using System.Linq.Expressions;

namespace FarmFresh.Repositories;

internal sealed class UserRepository(FarmFreshDbContext data) :
    RepositoryBase<ApplicationUser>(data), IUserRepository
{
    public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
    {
        await CreateAsync(user);
        return user;
    }

    public void DeleteUser(ApplicationUser user) => Delete(user);

    public IQueryable<ApplicationUser> FindUsersByConditionAsync(Expression<Func<ApplicationUser, bool>> expression, bool trackChanges) =>
           FindByCondition(expression, trackChanges);

    public void UpdateUser(ApplicationUser user) => Update(user);

    public async Task<ApplicationUser?> GetUserByIdAsync(Guid id) => await GetByIdAsync(id);
}
