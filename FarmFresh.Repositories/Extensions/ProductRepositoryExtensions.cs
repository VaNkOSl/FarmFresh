using FarmFresh.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Repositories.Extensions;

public static class ProductRepositoryExtensions
{
    public static IQueryable<Product> FilterProductsByPrice(this IQueryable<Product> products, decimal minPrice, decimal maxPrice) =>
        products.Where(p => (p.Price >= minPrice && p.Price <= maxPrice));
    
    public static IQueryable<Product> Search(this IQueryable<Product> products, string searchTerm)
    {
        if(string.IsNullOrWhiteSpace(searchTerm))
        {
            return products;
        }

        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return products
            .Where(p => p.Name.ToLower().Contains(lowerCaseTerm));
    }

    public static IQueryable<Product> GetProductsWithDetails(this IQueryable<Product> products) =>
      products.Include(c => c.Category)
              .Include(f => f.Farmer)
              .ThenInclude(u => u.User)
              .Include(ph => ph.ProductPhotos);
}
