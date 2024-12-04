using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories;

public interface IProductRepository
{
    Task<Product> CreateProductAsync(Product product);

    void DeleteProduct(Product product);

    void UpdateProduct(Product product);

    IQueryable<Product> FindProductByConditionAsync(Expression<Func<Product, bool>> condition, bool trackChanges);

    IQueryable<Product> FindAllProducts(bool trackChanges);
}
