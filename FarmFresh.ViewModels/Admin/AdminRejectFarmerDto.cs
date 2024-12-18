using FarmFresh.ViewModels.Email;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.ViewModels.Admin;

public record AdminRejectFarmerDto : AdminAllFarmerViewModel, IRejectEmailModel
{
    [EmailAddress]
    public string EmailTo { get; set; } = string.Empty;

    [EmailAddress]
    public string EmailFrom { get; set; } = string.Empty;

    public string? EmailSubject { get; set; }

    public string? EmailBody { get; set; }

    public string FarmerEmail { get; set; } = string.Empty;
    public string? Name { get; set; }
}
