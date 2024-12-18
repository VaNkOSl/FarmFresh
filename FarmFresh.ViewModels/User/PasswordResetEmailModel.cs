using FarmFresh.ViewModels.Email;

namespace FarmFresh.ViewModels.User;

public record PasswordResetEmailModel : IRejectEmailModel
{
    public string EmailFrom { get; set; } = string.Empty;

    public string EmailTo { get; set; }  = string.Empty;

    public string EmailSubject { get; set; } = string.Empty;

    public string EmailBody { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
