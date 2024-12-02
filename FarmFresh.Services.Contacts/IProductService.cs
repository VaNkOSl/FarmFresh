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
        Task<ProductViewModel?> GetProductByIdAsync(Guid productId);
        Task AddProductAsync(CreateProductViewModel model);
        Task UpdateProductAsync(EditProductViewModel model);
        Task DeleteProductAsync(Guid productId);
        Task<IEnumerable<ProductViewModel>> GetPagedProductsAsync(int pageIndex, int pageSize, string filter);
    }


}
