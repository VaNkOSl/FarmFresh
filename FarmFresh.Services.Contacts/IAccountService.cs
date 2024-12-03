using FarmFresh.ViewModels.User;

namespace FarmFresh.Services.Contacts;

public interface IAccountService
{
    Task<bool> Register(RegisterViewModel model, bool trackChanges);

    Task<bool> Login(LoginViewModel model, bool trackChanges);

    Task Logout();

    Task<ProfileViewModel> GetUserProfileAsync(string userId, bool trackChanges);

    Task<bool> DoesUserExistAsync(string userName, string email, bool trackChanges);

    Task DeleteUserAsync(Guid userId, bool trackChanges);

    Task<UserForUpdateDto> GetUserForUpdateAsync(Guid userId, bool trackChanges);

    Task UpdateUserAsync(UserForUpdateDto model, bool trackChanges);
}
