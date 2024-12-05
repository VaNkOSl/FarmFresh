using FarmFresh.Data.Models;

namespace FarmFresh.Repositories.Extensions;

public static class ProductRepositoryExtensions
{
    public static IQueryable<Product> FilterProductsByPrice(this IQueryable<Product> products, decimal minPrice, decimal maxPrice) =>
        products.Where(p => (p.Price >= minPrice && p.Price <= maxPrice));

    public static IQueryable<Product> FilterByPriceAscending(this IQueryable<Product> products, string orderByPrice) => 
        orderByPrice switch
        {
            "Ascending" => products.OrderBy(p => p.Price),
            "Descending" => products.OrderByDescending(p => p.Price),
            _ => products 
        };
    
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
}
