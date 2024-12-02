using FarmFresh.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services.Contacts
{
    using FarmFresh.ViewModels;

    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task<ProductViewModel?> GetProductByIdAsync(Guid productId);
        Task<PagedResult<ProductViewModel>> GetPagedProductsAsync(string? filter, int pageIndex, int pageSize);
        Task AddProductAsync(CreateProductViewModel model);
        Task UpdateProductAsync(EditProductViewModel model);
        Task DeleteProductAsync(Guid productId);
    }


}
