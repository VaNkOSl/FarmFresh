using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using System.Linq.Expressions;

namespace FarmFresh.Repositories;

internal sealed class ProductPhotoRepository(FarmFreshDbContext data, IValidateEntity validateEntity)
    : RepositoryBase<ProductPhoto>(data), IProductPhotoRepository
{
    public async Task<ProductPhoto> CreateProductPhotoAsync(ProductPhoto productPhoto)
    {
        await CreateAsync(productPhoto);
        return productPhoto;
    }

    public void DeleteProductPhoto(ProductPhoto productPhoto) => Delete(productPhoto);

    public void UpdateProductPhoto(ProductPhoto productPhoto) => Update(productPhoto);

    public IQueryable<ProductPhoto> FindProductPhotoByConditionAsync(Expression<Func<ProductPhoto, bool>> condition, bool trackChanges) =>
        FindByCondition(condition, trackChanges);
}
