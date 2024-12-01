using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static FarmFresh.Commons.EntityValidationConstants.User;

namespace FarmFresh.ViewModels.User;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "First Name")]
    [StringLength(UserFirstNameMaxLength, MinimumLength = UserFirstNameMinLength)]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [Display(Name = "Last Name")]
    [StringLength(UserLastNameMaxLength, MinimumLength = UserLastNameMinLength)]
    public string LastName { get; init; } = string.Empty;

    [Required]
    [Display(Name = "Username")]
    [StringLength(UserUserNameMaxLength, MinimumLength = UserUserNameMinLength)]
    public string UserName { get; init; } = string.Empty;

    [Required]
    [Display(Name = "Email")]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; init; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; init; } = string.Empty;

    public DateTime DateOfBirth { get; init; }

    public string FarmDescription { get; init; } = string.Empty;

    public string Location { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string UserType { get; init; } = "Normal";

    public IFormFile? PhotoFile { get; init; }
}
