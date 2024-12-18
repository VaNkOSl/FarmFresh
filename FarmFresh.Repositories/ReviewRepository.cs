using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using System.Linq.Expressions;

namespace FarmFresh.Repositories;

internal sealed class ReviewRepository(FarmFreshDbContext data, IValidateEntity validateEntity)
    : RepositoryBase<Review>(data), IReviewRepository
{
    public async Task<Review> CreateReviewAsync(Review review)
    {
       await CreateAsync(review);
        return review;
    }

    public void DeleteReview(Review review) => Delete(review);

    public void UpdateReview(Review review) => Update(review);

    public IQueryable<Review> FindReviewByConditionAsync(Expression<Func<Review, bool>> condition, bool trackChanges) =>
        FindByCondition(condition, trackChanges);

    public IQueryable<Review> GetAllReviews(bool trackChanges) =>
        FindAll(trackChanges);
}
