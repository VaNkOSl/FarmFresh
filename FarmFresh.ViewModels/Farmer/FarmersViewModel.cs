namespace FarmFresh.ViewModels.Farmer;

public record FarmersViewModel(
    Guid Id,
    string FullName,
    string PhoneNumber,
    string PhotoString,
    byte[] Photo,
    int ProductCount)
{
}
