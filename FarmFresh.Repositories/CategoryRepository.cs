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

    public void DeleteCategory(Category category) => Delete(category);

    public void UpdateCategory(Category category) => Update(category);

    public IQueryable<Category> FindCategoryByConditionAsync(Expression<Func<Category, bool>> expression, bool trackChanges) =>
           FindByCondition(expression, trackChanges);

    public IQueryable<Category> GetAllCategories(bool trackChanges) =>
        FindAll(trackChanges);
}
