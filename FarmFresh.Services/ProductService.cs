using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Contacts.ProductsInterfaces;
using FarmFresh.ViewModels.Product;

namespace FarmFresh.Services;

internal sealed class ProductService : IProductService
{
    private readonly IProductManagmentService _productManagmentService;
    private readonly IProductsQueryService _productsQueryService;

    public ProductService(
        IProductManagmentService productManagmentService,
        IProductsQueryService productsQueryService)
    {
        _productManagmentService = productManagmentService;
        _productsQueryService = productsQueryService;
    }

    public async Task CreateProductAsync(CreateProductDto model, string userId, bool trackChanges) =>
       await _productManagmentService.CreateProductAsync(model, userId, trackChanges);

    public async Task<CreateProductDto> PrepareCreateProductModelAsync(bool trackChanges) =>
        await _productsQueryService.PrepareCreateProductModelAsync(trackChanges);

    public async Task<ProductsListViewModel> CreateProductsViewModelAsync(IEnumerable<AllProductsDto> allProducts, MetaData metaData, string? searchTerm) =>
       await _productsQueryService.CreateProductsViewModelAsync(allProducts, metaData, searchTerm);

    public async Task<(IEnumerable<AllProductsDto> products, MetaData metaData)> GetAllProductsAsync(ProductParameters parameters, bool trackChanges) =>
        await _productsQueryService.GetAllProductsAsync(parameters, trackChanges);

    public async Task DeleteProductAsync(Guid productId, bool trackChanges) =>
       await _productManagmentService.DeleteProductAsync(productId, trackChanges);

    public async Task<ProductPreDeleteDto> GetProductForDeletingAsync(Guid productId, bool trackChanges) =>
        await _productsQueryService.GetProductForDeletingAsync(productId, trackChanges);

    public async Task<UpdateProductDto> GetProductForUpdateAsync(Guid productId, bool trackChanges) =>
        await _productsQueryService.GetProductForUpdateAsync(productId, trackChanges);

    public async Task UpdateProductAsync(UpdateProductDto model, Guid productId, bool trackChanges) =>
       await _productManagmentService.UpdateProductAsync(model, productId, trackChanges);

    public async Task<ProductDetailsDto> GetProductDetailsAsync(Guid productId, bool trackChanges) =>
       await _productsQueryService.GetProductDetailsAsync(productId, trackChanges);


    public async Task<IEnumerable<MineProductsDto>> GetAllFarmersProductByFarmerIdAsync(Guid farmerId, bool trackChanges) =>
        await _productsQueryService.GetAllFarmersProductByFarmerIdAsync(farmerId, trackChanges);
}