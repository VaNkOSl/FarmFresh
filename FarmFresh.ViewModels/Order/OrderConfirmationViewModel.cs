using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Product;

namespace FarmFresh.ViewModels.Order;

public record OrderConfirmationViewModel(
    Guid Id,
    decimal Price,
    decimal ShipmentPrice,
    int Quantity,
    decimal TotalPrice,
    string FirstName,
    string LastName,
    string Adress,
    string PhoneNumber,
    string Email,
    ICollection<OrderProduct> Products,
    IEnumerable<ProductPhotosDto> Photos,
    IEnumerable<CartItemViewModel> CartItems);