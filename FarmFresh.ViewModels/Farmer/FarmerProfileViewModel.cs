namespace FarmFresh.ViewModels.Farmer;

public record FarmerProfileViewModel(
    string FullName,
    string PhoneNumber,
    string Location,
    string FarmDescription,
    string? PhotoString,
    Guid Id)
{
}
