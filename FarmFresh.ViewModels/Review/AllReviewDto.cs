using FarmFresh.ViewModels.Product;

namespace FarmFresh.ViewModels.Review;

public record AllReviewDto(
    Guid Id,
    string ProductName,
    Guid ProductId,
    ICollection<ProductPhotosDto> Photos);
