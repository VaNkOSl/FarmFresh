using FarmFresh.ViewModels.Product;

namespace FarmFresh.ViewModels.Order;

public record CartItemViewModel(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal Price,
    decimal TotalPrice,
    IEnumerable<ProductPhotosDto> Photos);