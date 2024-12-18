using FarmFresh.ViewModels.Product;

namespace FarmFresh.ViewModels.Order;

public record OrderListViewModel(
    Guid Id,
    Guid OrderId,
    string ProductName,
    string OrderStatus,
    decimal Price,
    int Quantity,
    IEnumerable<ProductPhotosDto> Photos);