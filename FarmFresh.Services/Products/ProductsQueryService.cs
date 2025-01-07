using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.Extensions;
using FarmFresh.Services.Contacts.ProductsInterfaces;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Categories;
using FarmFresh.ViewModels.Product;
using LoggerService.Contacts;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services.Products;

public class ProductsQueryService : IProductsQueryService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public ProductsQueryService(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager,
        IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task<ProductsListViewModel> CreateProductsViewModelAsync(IEnumerable<AllProductsDto> allProducts, MetaData metaData, string? searchTerm) =>
        _mapper.Map<ProductsListViewModel>((allProducts, metaData, searchTerm));

    public async Task<IEnumerable<MineProductsDto>> GetAllFarmersProductByFarmerIdAsync(Guid farmerId, bool trackChanges)
    {
        var product = await
            _repositoryManager
            .ProductRepository
            .FindProductByConditionAsync(p => p.FarmerId == farmerId, trackChanges)
            .Include(ph => ph.ProductPhotos)
            .Include(f => f.Farmer)
            .Include(c => c.Category)
            .ToListAsync();

        return _mapper.Map<IEnumerable<MineProductsDto>>(product);
    }

    public async Task<(IEnumerable<AllProductsDto> products, MetaData metaData)> GetAllProductsAsync(ProductParameters parameters, bool trackChanges)
    {
        var productWithMetaData = await
            _repositoryManager
            .ProductRepository
            .GetProductsAsync(parameters, trackChanges);

        var productDto = _mapper.Map<IEnumerable<AllProductsDto>>(productWithMetaData);

        return (products: productDto, metaData: productWithMetaData.MetaData);
    }

    public async Task<ProductDetailsDto> GetProductDetailsAsync(Guid productId, bool trackChanges)
    {
        var product = await GetProductAsync(productId, trackChanges);

        ProductHelper.CheckProductNotFound(product, productId, nameof(GetProductDetailsAsync), _loggerManager);

        return _mapper.Map<ProductDetailsDto>(product);
    }

    public async Task<ProductPreDeleteDto> GetProductForDeletingAsync(Guid productId, bool trackChanges)
    {
        var productForDeleting = await GetProductAsync(productId, trackChanges);

        ProductHelper.CheckProductNotFound(productForDeleting, productId, nameof(GetProductForDeletingAsync), _loggerManager);

        return _mapper.Map<ProductPreDeleteDto>(productForDeleting);
    }

    public async Task<UpdateProductDto> GetProductForUpdateAsync(Guid productId, bool trackChanges)
    {
        var productToEdit = await GetProductAsync(productId, trackChanges);

        ProductHelper.CheckProductNotFound(productToEdit, productId, nameof(GetProductForUpdateAsync), _loggerManager);

        var category = await
                    _repositoryManager
                    .CategoryRepository
                    .GetAllCategoriesAsync(trackChanges);

        var productDto = _mapper.Map<UpdateProductDto>(productToEdit);
        productDto.Categories = category;
        return productDto;
    }

    public async Task<CreateProductDto> PrepareCreateProductModelAsync(bool trackChanges)
    {
        var categories = await
            _repositoryManager
            .CategoryRepository
            .GetAllCategories(trackChanges)
            .Include(p => p.Products)
            .ToListAsync();

        return new CreateProductDto { Categories = _mapper.Map<IEnumerable<AllCategoriesDTO>>(categories) };
    }

    private async Task<Product?> GetProductAsync(Guid productId, bool trackChanges) =>
         await _repositoryManager
         .ProductRepository
         .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
         .Include(ph => ph.ProductPhotos)
         .Include(f => f.Farmer)
         .ThenInclude(u => u.User)
         .Include(r => r.Reviews)
         .Include(c => c.Category)
         .FirstOrDefaultAsync();
}
