using FarmFresh.Commons.RequestFeatures;
using FarmFresh.ViewModels.Product;

namespace FarmFresh.Services.Contacts.ProductsInterfaces;

public interface IProductsQueryService
{
    Task<UpdateProductDto> GetProductForUpdateAsync(Guid productId, bool trackChanges);

    Task<ProductPreDeleteDto> GetProductForDeletingAsync(Guid productId, bool trackChanges);

    Task<(IEnumerable<AllProductsDto> products, MetaData metaData)> GetAllProductsAsync(ProductParameters parameters, bool trackChanges);

    Task<ProductsListViewModel> CreateProductsViewModelAsync(IEnumerable<AllProductsDto> allProducts, MetaData metaData, string? searchTerm);

    Task<CreateProductDto> PrepareCreateProductModelAsync(bool trackChanges);

    Task<ProductDetailsDto> GetProductDetailsAsync(Guid productId, bool trackChanges);

    Task<IEnumerable<MineProductsDto>> GetAllFarmersProductByFarmerIdAsync(Guid farmerId, bool trackChanges);
}
