using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories;

public interface ICategoryRepository
{
    Task<Category> CreateCategoryAsync(Category category);

    Task<Category?> GetCategoryByIdAsync(Guid id);

    void DeleteCategory(Category category);

    void UpdateCategory(Category category);

    IQueryable<Category> FindCategoryByConditionAsync(Expression<Func<Category, bool>> expression, bool trackChanges);
}
