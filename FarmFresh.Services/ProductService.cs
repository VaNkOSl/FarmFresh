﻿using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Categories;
using FarmFresh.ViewModels.Product;
using LoggerService.Contacts;
using LoggerService.Exceptions.InternalError.Product;
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

    public async Task<ProductDetailsDto> GetProductDetailsAsync(Guid productId, bool trackChanges)
    {
        var product = await GetProductAsync(productId, trackChanges);

       ProductHelper.CheckProductNotFound(product, productId, nameof(GetProductDetailsAsync), _loggerManager);

        return _mapper.Map<ProductDetailsDto>(product);
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

        try
        {
            return _mapper.Map<IEnumerable<MineProductsDto>>(product);
        }
        catch (Exception)
        {

            throw;
        }
    }
}