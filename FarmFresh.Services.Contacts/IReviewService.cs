using FarmFresh.ViewModels.Review;

namespace FarmFresh.Services.Contacts;

public interface IReviewService
{
    Task CreateProductReviewAsync(ProductReviewCreateDto model, Guid userId, bool trackChanges);

    Task<IEnumerable<AllReviewDto>> GetAllReviewsAsync(Guid userId, bool trackChanges);

    Task<IEnumerable<ProductReviewDto>> GetReviewedProductsAsync(Guid userId, bool trackChanges);
}
