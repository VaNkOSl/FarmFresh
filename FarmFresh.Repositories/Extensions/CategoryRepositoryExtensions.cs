using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.ViewModels.Categories;
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

    public static async Task<IEnumerable<AllCategoriesDTO>> GetAllCategoriesAsync(
        this ICategoryRepository categoryRepository,
        bool trackChanges)
    {
        var categories = categoryRepository
                         .GetAllCategories(trackChanges);

        return await categories
          .Select(category => new AllCategoriesDTO(category.Id, category.Name, category.Products.Count()))
          .ToListAsync();
    }
}
