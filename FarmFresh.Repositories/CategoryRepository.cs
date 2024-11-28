using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using System.Linq.Expressions;

namespace FarmFresh.Repositories;

internal sealed class CategoryRepository(FarmFreshDbContext data, IValidateEntity validateEntity) 
    : RepositoryBase<Category>(data), ICategoryRepository
{
    public async Task<Category> CreateCategoryAsync(Category category)
    {
        await CreateAsync(category);
        return category;
    }

    public void DeleteCategory(Category category) => DeleteCategory(category);

    public void UpdateCategory(Category category) => UpdateCategory(category);

    public async Task<Category?> GetCategoryByIdAsync(Guid id) => await GetByIdAsync(id);

    public IQueryable<Category> FindCategoryByConditionAsync(Expression<Func<Category, bool>> expression, bool trackChanges) =>
           FindByCondition(expression, trackChanges);
}
