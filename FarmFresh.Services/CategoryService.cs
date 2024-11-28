using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Category;
using LoggerService.Contacts;
using LoggerService.Exceptions.BadRequest;
using LoggerService.Exceptions.InternalError;
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

    public async Task CreateCategoryAsync(CategoryCreateUpdateForm model, bool trackChanges)
    {
        if (await DoesCategoryExistsAsync(model.Name, trackChanges))
        {
            _loggerManager.LogError($"[{nameof(CreateCategoryAsync)}] Category creation failed: Name '{model.Name}' already exists.");
            throw new CategoryNameAlreadyExists();
        }

        try
        {
            var category = _mapper.Map<Category>(model);
            await _repositoryManager.CategoryRepository.CreateCategoryAsync(category);
            await _repositoryManager.SaveAsync(category);
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new CategorySomethingWentWrong();
        }
    }

    public async Task<bool> DoesCategoryExistsAsync(string name, bool trackChanges)
    {
        var categories = _repositoryManager
                              .CategoryRepository
                              .FindCategoryByConditionAsync(c => c.Name == name, trackChanges);

        return await categories.AnyAsync();
    }
}
