using FarmFresh.ViewModels.Product;

namespace FarmFresh.ViewModels.Order;

public record OrderItemViewModel
{
    public OrderItemViewModel()
    {
        Photos = new HashSet<ProductPhotosDto>();
    }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string PhotoString { get; set; }

    public IEnumerable<ProductPhotosDto> Photos { get; set; }
}
