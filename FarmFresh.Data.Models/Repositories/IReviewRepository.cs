using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories;

public interface IReviewRepository
{
    Task<Review> CreateReviewAsync(Review review);

    public void DeleteReview(Review review);

    public void UpdateReview(Review review);

    IQueryable<Review> FindReviewByConditionAsync(Expression<Func<Review, bool>> condition, bool trackChanges);

    IQueryable<Review> GetAllReviews(bool trackChanges);
}
