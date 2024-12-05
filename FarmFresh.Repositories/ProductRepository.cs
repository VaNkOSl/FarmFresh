using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using FarmFresh.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
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

    public async Task<PagedList<Product>> GetProductsAsync(ProductParameters productParameters, bool trackChanges)
    {
        var products = await
            FindAllProducts(trackChanges)
            .FilterProductsByPrice(productParameters.MinPrice, productParameters.MaxPrice)
            .FilterByPriceAscending(productParameters.OrderByPrice)
            .Search(productParameters.SearchTerm)
            .Include(p => p.ProductPhotos)
            .ToListAsync();

        return PagedList<Product>
               .ToPagedList(products, productParameters.PageNumber, productParameters.PageSize);
    }
}
