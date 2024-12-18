namespace FarmFresh.ViewModels.Email;

public interface IRejectEmailModel
{
    string EmailFrom { get; set; }

    string EmailTo { get; set; }

    string EmailSubject { get; set; }

    string EmailBody { get; set; }

    string Name { get; set; }
}
