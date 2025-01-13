namespace FarmFresh.ViewModels.User;

public record ChangePasswordViewModel(Guid Id,string Name, string NewPassword, string ConfirmNewPassword)
{
}
