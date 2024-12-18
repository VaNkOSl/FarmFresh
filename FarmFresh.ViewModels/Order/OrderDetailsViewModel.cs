using FarmFresh.Data.Models.Enums;
using FarmFresh.ViewModels.Product;

namespace FarmFresh.ViewModels.Order;

public record OrderDetailsViewModel(
    Guid Id,
    Guid OrderId,
    int Quantity,
    decimal Price,
    string FirstName,
    string LastName,
    string Adress,
    string PhoneNumber,
    string Email,
    string ProductName,
    DateTime CreatedDate,
    DeliveryOption DeliveryOption,
    OrderStatus OrderStatus,
    string ProductDescription,
    string Origin,
    string FarmerName,
    decimal ProductPrice,
    Seasons Seasons,
    DateTime HarvestDate,
    DateTime ExpirationDate,
    IEnumerable<ProductPhotosDto> Photos);