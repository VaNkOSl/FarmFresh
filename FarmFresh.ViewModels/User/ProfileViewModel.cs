using System.ComponentModel.DataAnnotations;
using static FarmFresh.Commons.EntityValidationConstants.User;

namespace FarmFresh.ViewModels.User;

public class ProfileViewModel
{
    [StringLength(UserFirstNameMaxLength, MinimumLength = UserFirstNameMinLength)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(UserLastNameMaxLength, MinimumLength = UserLastNameMinLength)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [StringLength(UserUserNameMaxLength, MinimumLength = UserUserNameMinLength)]
    public string UserName { get; set; } = string.Empty;
}
