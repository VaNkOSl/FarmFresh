using FarmFresh.ViewModels;
using FarmFresh.ViewModels.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FarmFresh.Services.Contacts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
    }
}
