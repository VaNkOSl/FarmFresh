namespace FarmFresh.ViewModels.Product;

public record AllProductsDto(
    Guid Id,
    string Name,
    decimal Price,
    int StockQuantity,
    IEnumerable<ProductPhotosDto> Photos);