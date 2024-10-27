using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static FarmFresh.Commons.EntityValidationConstants.User;

namespace FarmFresh.ViewModels.User;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "First Name")]
    [StringLength(UserFirstNameMaxLength, MinimumLength = UserFirstNameMinLength)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Last Name")]
    [StringLength(UserLastNameMaxLength, MinimumLength = UserLastNameMinLength)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Username")]
    [StringLength(UserUserNameMaxLength, MinimumLength = UserUserNameMinLength)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Email")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string FarmDescription { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string UserType { get; set; } = "Normal";

    public IFormFile? PhotoFile { get; set; }
}
