using FarmFresh.Commons.RequestFeatures;
using FarmFresh.ViewModels.Product;

namespace FarmFresh.Services.Contacts;

public interface IProductService
{
    Task CreateProductAsync(CreateProductDto model, string userId, bool trackChanges);

    Task<CreateProductDto> PrepareCreateProductModelAsync(bool  trackChanges);

    Task<(IEnumerable<AllProductsDto> products, MetaData metaData)> GetAllProductsAsync(ProductParameters parameters, bool trackChanges);

    Task <ProductsListViewModel> CreateProductsViewModelAsync(IEnumerable<AllProductsDto> allProducts, MetaData metaData, string? searchTerm);
}
