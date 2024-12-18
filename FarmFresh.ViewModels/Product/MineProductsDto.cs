using FarmFresh.Data.Models.Enums;

namespace FarmFresh.ViewModels.Product;

public record MineProductsDto(
    Guid Id,
    string Name,
    decimal Price,
    int StockQuantity,
    Status ProductStatus,
    string CategoryName,
    Guid FarmerId,
    ICollection<ProductPhotosDto> Photos);
