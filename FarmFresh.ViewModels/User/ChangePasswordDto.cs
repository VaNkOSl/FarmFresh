namespace FarmFresh.ViewModels.User;

public record ChangePasswordDto(
    Guid Id,
    string currentPassword,
    string changedPassword)
{
}
