using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts.ProductsInterfaces;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Product;
using LoggerService.Contacts;
using LoggerService.Exceptions.InternalError.Product;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services.Products;

public sealed class ProductManagmentService : IProductManagmentService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public ProductManagmentService(
        IRepositoryManager repositoryManager,
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

    public async Task DeleteProductAsync(Guid productId, bool trackChanges)
    {
        var productForDeleting = await GetProductAsync(productId, trackChanges);

        ProductHelper.CheckProductNotFound(productForDeleting, productId, nameof(DeleteProductAsync), _loggerManager);

        try
        {
            await ProductHelper.DeleteProductPhotoAsync(_repositoryManager, _loggerManager, productId, trackChanges);
            await ProductHelper.DeleteProductReviewAsync(_repositoryManager, _loggerManager, productId, trackChanges);

            _repositoryManager.ProductRepository.DeleteProduct(productForDeleting);
            await _repositoryManager.SaveAsync();
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new DeleteProductException();
        }
    }

    public async Task UpdateProductAsync(UpdateProductDto model, Guid productId, bool trackChanges)
    {
        var product = await GetProductAsync(productId, trackChanges);

        ProductHelper.CheckProductNotFound(product, productId, nameof(UpdateProductAsync), _loggerManager);

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
            throw new UpdateProductException();
        }
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
