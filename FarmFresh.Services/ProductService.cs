using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Categories;
using FarmFresh.ViewModels.Product;
using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;
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
        var farmer = await ValidateUserAndFarmerAsync(userId, trackChanges);
        await ValidateCategoryAsync(model.CategoryId, trackChanges);

        try
        {
            var product = _mapper.Map<Product>(model);
            product.FarmerId = farmer.Id;

            await _repositoryManager.ProductRepository.CreateProductAsync(product);
            await _repositoryManager.SaveAsync(product);
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

    private async Task<Farmer> ValidateUserAndFarmerAsync(string userId, bool trackChanges)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            _loggerManager.LogError($"[{nameof(CreateProductAsync)}] User with id {userId} was not found!");
            throw new UserIdNotFoundException(Guid.Parse(userId));
        }

        var farmer = await _repositoryManager
            .FarmerRepository
            .FindFarmersByConditionAsync(f => f.UserId.ToString() == userId, trackChanges)
            .FirstOrDefaultAsync();

        if (farmer is null)
        {
            _loggerManager.LogError($"[{nameof(CreateProductAsync)}] Farmer with id {farmer.Id} was not found!");
            throw new FarmerIdNotFoundException(Guid.Parse(userId));
        }

        return farmer;
    }

    private async Task ValidateCategoryAsync(Guid categoryId, bool trackChanges)
    {
        if (!await _repositoryManager.CategoryRepository.DoesCategoryExistByIdAsync(categoryId, trackChanges))
        {
            _loggerManager.LogError($"[{nameof(ValidateCategoryAsync)}] Category creation failed: Category with Id '{categoryId}' not found.");
            throw new CategoryIdNotFoundException(categoryId);
        }
    }
}
