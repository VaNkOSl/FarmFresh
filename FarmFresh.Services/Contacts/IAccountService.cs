using FarmFresh.ViewModels.User;

namespace FarmFresh.Services.Contacts;

public interface IAccountService
{
    Task<bool> Register(RegisterViewModel model);

    Task<bool> Login(LoginViewModel model);

    Task Logout();

    Task<ProfileViewModel> GetUserProfileAsync(string userId);
}
