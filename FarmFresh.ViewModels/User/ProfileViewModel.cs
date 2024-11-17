namespace FarmFresh.ViewModels.User;

public record class ProfileViewModel(
    string FirstName,
    string LastName,
    string Email,
    string UserName)
{
}
