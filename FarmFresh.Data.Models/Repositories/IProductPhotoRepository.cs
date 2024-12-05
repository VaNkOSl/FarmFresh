namespace FarmFresh.Data.Models.Repositories;

public interface IProductPhotoRepository
{
    Task<ProductPhoto> CreateProductPhotoAsync(ProductPhoto productPhoto);

    void DeleteProductPhoto(ProductPhoto productPhoto);

    void UpdateProductPhoto(ProductPhoto productPhoto);
}
