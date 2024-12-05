using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Categories;
using FarmFresh.ViewModels.Farmer;
using FarmFresh.ViewModels.Product;
using LoggerService.Contacts;
using LoggerService.Exceptions.Product;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services;

internal sealed class ProductService : IProductService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public ProductService(IRepositoryManager repositoryManager,
                          ILoggerManager loggerManager,
                          IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task CreateProductAsync(CreateProductDto model, string userId, bool trackChanges)
    {
        var farmer = await ProductHelper.ValidateUserAndFarmerAsync(userId, trackChanges, _repositoryManager, _loggerManager);
        await ProductHelper.ValidateCategoryAsync(model.CategoryId, trackChanges, _repositoryManager, _loggerManager);

        try
        {
            var product = _mapper.Map<Product>(model);
            product.FarmerId = farmer.Id;

            await _repositoryManager.ProductRepository.CreateProductAsync(product);
            await _repositoryManager.SaveAsync();

            if (model.Photos != null && model.Photos.Any())
            {
                var uploadDirectory = Path.Combine("wwwroot", "uploads");

                await ProductHelper.UploadProductPhotosAsync(model.Photos, product, uploadDirectory, _repositoryManager, _loggerManager);
            }

            _loggerManager.LogInfo($"[{nameof(CreateProductAsync)}] Product with Id {product.Id} and Name {product.Name} was successfully created by user with Id {userId} at Date: {DateTime.UtcNow}");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new CreateProductException();
        }
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

    public async Task<ProductsListViewModel> CreateProductsViewModelAsync(IEnumerable<AllProductsDto> allProducts, MetaData metaData, string? searchTerm) =>
        _mapper.Map<ProductsListViewModel>((allProducts, metaData, searchTerm));

    public async Task<FarmersListViewModel> CreateFarmersListViewModelAsync(IEnumerable<FarmersViewModel> farmers, MetaData metaData, string? searchTerm) =>
     _mapper.Map<FarmersListViewModel>((farmers, metaData, searchTerm));

    public async Task<(IEnumerable<AllProductsDto> products, MetaData metaData)> GetAllProductsAsync(ProductParameters parameters, bool trackChanges)
    {
        var productWithMetaData = await 
            _repositoryManager
            .ProductRepository
            .GetProductsAsync(parameters, trackChanges);

        var productDto = _mapper.Map<IEnumerable<AllProductsDto>>(productWithMetaData);

        return (products: productDto, metaData: productWithMetaData.MetaData);
    }
}
