using FarmFresh.ViewModels.Category;

namespace FarmFresh.Services.Contacts;

public interface ICategoryService
{
    Task CreateCategoryAsync(CategoryCreateForm model, bool trackChanges);

    Task<bool> DoesCategoryExistsByNameAsync(string name, bool trackChanges);

    Task<bool> DeleteCategory(Guid categoryId, bool trackChanges);

    Task UpdateCategory(CategoryUpdateForm model,Guid categoryId, bool trackChanges);

    Task<CategoryUpdateForm> GetCategoryForUpdate(Guid categoryId, bool trackChanges);

    Task<IEnumerable<AllCategoriesDTO>> GetAllCategoriesAsync(bool trackChanges);
}
