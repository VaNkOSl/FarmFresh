using FarmFresh.Infrastructure.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.ViewModels.Farmer;

public class FarmerCreateUpdateForm
{
    [Required]
    [Display(Name = "Please enter information about your farm!")]
    public string FarmDescription { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Please enter the location of your farm!")]
    public string Location { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Please enter your valid phone number!")]
    public string PhoneNumber { get; set; } = string.Empty;

    public byte[] Photo { get; set; } = new byte[0];

    public string? PhotoString { get; set; }

    [DataType(DataType.Upload)]
    public IFormFile? PhotoFile { get; set; }

    [Required]
    [Display(Name = "Please enter your valid egn!")]
    public string Egn { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Please enter your date of birth!")]
    public DateTime BirthDate { get; set; }

    [PrivacyAccepted]
    public bool PrivacyAccepted { get; set; }
}
