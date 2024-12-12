using FarmFresh.ViewModels.Product;

namespace FarmFresh.ViewModels.Farmer;

public record FarmerDetailsDto(
    Guid Id,
    string FullName,
    string PhoneNumber,
    string PhotoString,
    byte[] Photo,
    string Email,
    string FarmDescription,
    string Location,
    IEnumerable<AllProductsDto> Products)
{
}
