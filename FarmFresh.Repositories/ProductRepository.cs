using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using System.Linq.Expressions;

namespace FarmFresh.Repositories;

internal sealed class ProductRepository(FarmFreshDbContext data, IValidateEntity validateEntity) 
    : RepositoryBase<Product>(data), IProductRepository
{
    public async Task<Product> CreateProductAsync(Product product)
    {
        await CreateAsync(product);
        return product;
    }

    public void DeleteProduct(Product product) => Delete(product);

    public void UpdateProduct(Product product) => Update(product);

    public IQueryable<Product> FindAllProducts(bool trackChanges) =>
        FindAll(trackChanges);

    public IQueryable<Product> FindProductByConditionAsync(Expression<Func<Product, bool>> condition, bool trackChanges) =>
        FindByCondition(condition, trackChanges);
}
