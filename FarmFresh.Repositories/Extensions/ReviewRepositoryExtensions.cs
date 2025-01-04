using FarmFresh.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Repositories.Extensions;

public static class ReviewRepositoryExtensions
{
    public static IQueryable<Review> GetProductReviewWithDetailsAsync(this IQueryable<Review> reviews) =>
        reviews.Include(p => p.Product)
            .ThenInclude(ph => ph.ProductPhotos)
            .Include(p => p.Product)
            .ThenInclude(f => f.Farmer)
            .ThenInclude(u => u.User);
}
