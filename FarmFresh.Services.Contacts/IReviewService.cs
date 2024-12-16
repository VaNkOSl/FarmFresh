using FarmFresh.ViewModels.Review;

namespace FarmFresh.Services.Contacts;

public interface IReviewService
{
    Task CreateProductReviewAsync(ProductReviewCreateDto model, Guid userId, bool trackChanges);

    Task DeleteReview(Guid reviewId, bool trackChanges);

    Task UpdatereviewAsync(ProductReviewUpdateDto model, Guid reviewId, bool trackChanges);
}
