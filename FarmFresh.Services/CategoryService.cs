using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Category;
using LoggerService.Contacts;
using LoggerService.Exceptions.BadRequest;
using LoggerService.Exceptions.InternalError.Categories;
using LoggerService.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services;

internal sealed class CategoryService : ICategoryService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public CategoryService(IRepositoryManager repositoryManager,
                        ILoggerManager loggerManager,
                        IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task CreateCategoryAsync(CategoryCreateForm model, bool trackChanges)
    {
        if (await DoesCategoryExistsByNameAsync(model.Name, trackChanges))
        {
            _loggerManager.LogError($"[{nameof(CreateCategoryAsync)}] Category creation failed: Name '{model.Name}' already exists.");
            throw new CategoryNameAlreadyExists();
        }

        try
        {
            var category = _mapper.Map<Category>(model);
            await _repositoryManager.CategoryRepository.CreateCategoryAsync(category);
            await _repositoryManager.SaveAsync(category);
            _loggerManager.LogInfo($"Successfully created category with name {model.Name}.");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new CategorySomethingWentWrong();
        }
    }

    public async Task<bool> DeleteCategory(Guid categoryId, bool trackChanges)
    {
        var categoryForDeleting = await _repositoryManager
            .CategoryRepository
            .FindCategoryByConditionAsync(c => c.Id == categoryId, trackChanges)
            .FirstOrDefaultAsync();

        if(categoryForDeleting is null)
        {
            _loggerManager.LogError($"[{nameof(DeleteCategory)}] Category with ID {categoryId} not found.");
        }

        try
        {
            _repositoryManager.CategoryRepository.DeleteCategory(categoryForDeleting!);
            await _repositoryManager.SaveAsync();
            _loggerManager.LogInfo($"Category with ID {categoryId} successfully deleted.");
            return true;

        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"Error occurred while deleting category with ID {categoryId}: {ex.Message}");
            throw new DeleteCategorySomethingWentWrong();
        }
    }

    public async Task<bool> DoesCategoryExistsByNameAsync(string name, bool trackChanges) => 
        await _repositoryManager
        .CategoryRepository
        .FindCategoryByConditionAsync(c => c.Name == name, trackChanges)
        .AnyAsync();

    public async Task<IEnumerable<AllCategoriesDTO>> GetAllCategoriesAsync(bool trackChanges)
    {
        var categories = _repositoryManager
                         .CategoryRepository
                         .GetAllCategories(trackChanges);

        return await categories
          .Select(category => new AllCategoriesDTO(category.Id, category.Name, category.Products.Count()))
          .ToListAsync();
    }

    public async Task<CategoryUpdateForm> GetCategoryForUpdate(Guid categoryId, bool trackChanges)
    {
        var currentCategory = await _repositoryManager
            .CategoryRepository
            .FindCategoryByConditionAsync(c => c.Id == categoryId, trackChanges)
            .FirstOrDefaultAsync();

        if(currentCategory is null)
        {
            _loggerManager.LogError($"[{nameof(GetCategoryForUpdate)}] Category with id {categoryId} was not found! Time: {DateTime.UtcNow}");
            throw new CategoryIdNotFoundException(categoryId);
        }

        return _mapper.Map<CategoryUpdateForm>(currentCategory);
    }

    public async Task UpdateCategory(CategoryUpdateForm model, Guid categoryId, bool trackChanges)
    {
        var categoryForUpdate = await _repositoryManager
            .CategoryRepository
            .FindCategoryByConditionAsync(c => c.Id == categoryId, trackChanges)
            .FirstOrDefaultAsync();

        if (categoryForUpdate is null)
        {
            _loggerManager.LogError($"[{nameof(DeleteCategory)}] Category with ID {categoryId} not found.");
        }

        try
        {
            _mapper.Map(model, categoryForUpdate);
            _repositoryManager.CategoryRepository.UpdateCategory(categoryForUpdate!);
            await _repositoryManager.SaveAsync(categoryForUpdate!);
            _loggerManager.LogInfo($"Category with ID {categoryId} successfully updated.");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"Error occurred while deleting category with ID {categoryId}: {ex.Message}");
            throw new UpdateCategorySomethingWentWrong();
        }
    }
}
