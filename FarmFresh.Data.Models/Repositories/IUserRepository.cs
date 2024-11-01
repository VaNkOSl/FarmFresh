using FarmFresh.Data.Models;

namespace FarmFresh.Repositories.Contacts;

public interface IUserRepository
{
    Task<IQueryable<ApplicationUser>> GetAllUsersAsync();

    Task<IQueryable<ApplicationUser>> GetAllUserReadOnlyAsync();

    Task<ApplicationUser> CreateUserAsync(ApplicationUser user);

    Task<ApplicationUser?> GetUserByIdAsync(Guid id);

    Task UpdateUserAsync(ApplicationUser user);

    Task DeleteUserAsync(Guid id);
}
