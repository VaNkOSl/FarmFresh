using System.ComponentModel.DataAnnotations;

namespace FarmFresh.ViewModels.User;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Username or Email")]
    public string UserNameOrEmail { get; init; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; init; } = string.Empty;
}
