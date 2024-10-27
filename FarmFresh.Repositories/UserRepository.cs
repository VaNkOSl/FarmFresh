using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Repositories
{
    public class UserRepository(FarmFreshDbContext data) :
        RepositoryBase<ApplicationUser>(data), IUserRepository
    {
        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
        {
            await AddAsync(user);
            await data.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await DeleteAsync<ApplicationUser>(id);
            await data.SaveChangesAsync();
        }

        public Task<IQueryable<ApplicationUser>> GetAllUserReadOnlyAsync() => Task.FromResult(AllReadOnly<ApplicationUser>());

        public Task<IQueryable<ApplicationUser>> GetAllUsersAsync() => Task.FromResult(All<ApplicationUser>());

        public async Task<ApplicationUser?> GetUserByIdAsync(Guid id) => await GetByIdAsync<ApplicationUser>(id);

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            await UpdateAsync(user);
            await data.SaveChangesAsync();
        }
    }
}
