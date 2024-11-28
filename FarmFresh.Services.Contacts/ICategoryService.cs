using FarmFresh.ViewModels.Category;

namespace FarmFresh.Services.Contacts;

public interface ICategoryService
{
    Task CreateCategoryAsync(CategoryCreateUpdateForm model, bool trackChanges);

    Task<bool> DoesCategoryExistsAsync(string name, bool trackChanges);
}
