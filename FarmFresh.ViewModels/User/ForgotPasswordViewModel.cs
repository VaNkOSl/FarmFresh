using System.ComponentModel.DataAnnotations;

namespace FarmFresh.ViewModels.User;

public record ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
