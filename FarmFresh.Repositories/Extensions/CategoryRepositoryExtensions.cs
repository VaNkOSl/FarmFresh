using FarmFresh.Data.Models.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Repositories.Extensions;

public static class CategoryRepositoryExtensions
{
    public static async Task<bool> DoesCategoryExistByNameAsync(
        this ICategoryRepository categoryRepository,
        string name,
        bool trackChanges) =>
             await categoryRepository
            .FindCategoryByConditionAsync(c => c.Name == name, trackChanges)
            .AnyAsync();

    public static async Task<bool> DoesCategoryExistByIdAsync(
        this ICategoryRepository categoryRepository,
        Guid id,
        bool  trackChanges) =>
           await categoryRepository
            .FindCategoryByConditionAsync(c => c.Id == id, trackChanges)
            .AnyAsync();

}
