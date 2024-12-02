using FarmFresh.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services.Contacts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task<ProductViewModel> GetProductByIdAsync(int id);
        Task AddProductAsync(CreateProductViewModel createModel);
        Task UpdateProductAsync(EditProductViewModel editModel);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<ProductViewModel>> GetPagedProductsAsync(int pageIndex, int pageSize, string filter);
    }


}
