using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Admin;
using FarmFresh.ViewModels.Categories;
using FarmFresh.ViewModels.Product;
using LoggerService.Contacts;
using LoggerService.Exceptions.InternalError.Product;
using LoggerService.Exceptions.NotFound;
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
            product.Farmer = farmer;
            product.Farmer.OwnedProducts.Add(product);

            await _repositoryManager.ProductRepository.CreateProductAsync(product);
            await _repositoryManager.SaveAsync(product);

            if (model.Photos != null && model.Photos.Any())
            {
                var uploadDirectory = Path.Combine("wwwroot", "uploads");

                await ProductHelper
                    .UploadProductPhotosAsync(model.Photos,
                    product,
                    uploadDirectory,
                    _repositoryManager,
                    _loggerManager);
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

    public async Task<(IEnumerable<AllProductsDto> products, MetaData metaData)> GetAllProductsAsync(ProductParameters parameters, bool trackChanges)
    {
        var productWithMetaData = await 
            _repositoryManager
            .ProductRepository
            .GetProductsAsync(parameters, trackChanges);

        var productDto = _mapper.Map<IEnumerable<AllProductsDto>>(productWithMetaData);

        return (products: productDto, metaData: productWithMetaData.MetaData);
    }

    public async Task DeleteProductAsync(Guid productId, bool trackChanges)
    {
        var productForDeleting = await GetProductAsync(productId, trackChanges);

        if (productForDeleting is null)
        {
            _loggerManager.LogError($"[{nameof(DeleteProductAsync)}] Product with Id {productId} was not found at Date: {DateTime.UtcNow}");
            throw new ProductIdNotFoundException(productId);
        }

        try
        {
            await ProductHelper.DeleteProductPhotoAsync(_repositoryManager, _loggerManager, productId, trackChanges);

            _repositoryManager.ProductRepository.DeleteProduct(productForDeleting);
           await _repositoryManager.SaveAsync();
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new DeleteProductException();
        }
    }

    public async Task<ProductPreDeleteDto> GetProductForDeletingAsync(Guid productId, bool trackChanges)
    {
        var productForDeleting = await GetProductAsync(productId, trackChanges);

        if (productForDeleting is null)
        {
            _loggerManager.LogError($"[{nameof(GetProductForDeletingAsync)}] Product with Id {productId} was not found at Date: {DateTime.UtcNow}");
            throw new ProductIdNotFoundException(productId);
        }

        return _mapper.Map<ProductPreDeleteDto>(productForDeleting);
    }

    public async Task<UpdateProductDto> GetProductForUpdateAsync(Guid productId, bool trackChanges)
    {
        var productToEdit = await GetProductAsync(productId, trackChanges);

        if (productToEdit is null)
        {
            _loggerManager.LogError($"[{nameof(GetProductForUpdateAsync)}] Product with ID {productId} was not found!");
            throw new ProductIdNotFoundException(productId);
        }

        var category = await
                    _repositoryManager
                    .CategoryRepository
                    .GetAllCategoriesAsync(trackChanges);

        var productDto = _mapper.Map<UpdateProductDto>(productToEdit);
        productDto.Categories = category;
        return productDto;
    }

    public async Task UpdateProductAsync(UpdateProductDto model, Guid productId, bool trackChanges)
    {
        var product = await GetProductAsync(productId, trackChanges);

        if (product == null)
        {
            _loggerManager.LogError($"Product with ID {productId} was not found.");
            throw new ProductIdNotFoundException(productId);
        }

        try
        {
            _mapper.Map(model, product);

            _repositoryManager.ProductRepository.UpdateProduct(product);
            await _repositoryManager.SaveAsync(product);

            if (model.Photos != null && model.Photos.Any())
            {
                var uploadDirectory = Path.Combine("wwwroot", "uploads");

                await ProductHelper
                    .UploadProductPhotosAsync(model.Photos,
                    product,
                    uploadDirectory,
                    _repositoryManager,
                    _loggerManager);
            }

        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An error occurred while updating the product: {ex.Message}");
            throw;
        }
    }

    private async Task<Product?> GetProductAsync(Guid productId, bool trackChanges) =>
        await _repositoryManager
            .ProductRepository
            .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
            .Include(ph => ph.ProductPhotos)
            .Include(c => c.Category)
            .FirstOrDefaultAsync();
}