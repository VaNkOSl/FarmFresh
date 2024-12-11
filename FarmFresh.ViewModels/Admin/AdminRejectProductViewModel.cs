using FarmFresh.ViewModels.Email;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.ViewModels.Admin;

public record AdminRejectProductViewModel : AdminProductreject, IRejectEmailModel
{
    [EmailAddress]
    public string EmailFrom { get; set; } = string.Empty;

    [EmailAddress]
    public string EmailTo { get; set; } = string.Empty;

    public string EmailSubject { get; set; } = string.Empty;

    public string EmailBody { get; set; } = string.Empty;
}
