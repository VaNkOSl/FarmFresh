using FarmFresh.ViewModels.User;

namespace FarmFresh.Services.Contacts;

public interface IAccountService
{
    Task<bool> Register(RegisterViewModel model, bool trackChanges);

    Task<bool> Login(LoginViewModel model, bool trackChanges);

    Task Logout();

    Task<ProfileViewModel> GetUserProfileAsync(string userId);

    Task<bool> DoesUserExistAsync(string userName, string email, bool trackChanges);
}
