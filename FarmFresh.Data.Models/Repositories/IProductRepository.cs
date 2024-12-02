using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Repositories
{
    using FarmFresh.Data.Models;

    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(); // Get all products
        Task<Product?> GetProductByIdAsync(Guid productId); // Get a product by ID
        Task AddProductAsync(Product product); // Add a new product
        Task UpdateProductAsync(Product product); // Update an existing product
        Task DeleteProductAsync(Guid productId); // Delete a product by ID
        Task<IEnumerable<Product>> GetPagedProductsAsync(int pageIndex, int pageSize, string? filter); // Get products with pagination and filtering
        Task<int> GetTotalCountAsync(string? filter); // Get the total count of products for filtering
    }

}
