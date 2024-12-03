namespace FarmFresh.ViewModels.User;

public record UserForUpdateDto(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    Guid Id)
{
}
