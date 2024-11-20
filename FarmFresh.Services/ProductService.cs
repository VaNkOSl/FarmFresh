using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Infrastructure.TagHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services
{
    public class ProductService
    {
        private readonly FarmFreshDbContext _context;

        public ProductService(FarmFreshDbContext context)
        {
            _context = context;
        }

        // Add a new product
        public async Task AddProductAsync(Product product)
        {
            product.Id = Guid.NewGuid(); // Ensure unique ID
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Get all approved products with paging
        public async Task<List<Product>> GetApprovedProductsAsync(int page, int pageSize)
        {
            return await _context.Products
                .Where(p => p.IsApproved)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Reduce stock quantity
        public async Task<bool> ReduceStockAsync(Guid productId, int quantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) throw new ArgumentException("Product not found.");
            if (product.StockQuantity < quantity) throw new InvalidOperationException("Not enough stock.");

            product.StockQuantity -= quantity;

            // Check if product is out of stock
            if (product.StockQuantity == 0) product.IsAvailable = false;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }

        // Get product details by ID
        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Reviews)
                .Include(p => p.ProductPhotos)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<bool> UpdateStockAsync(Guid productId, int quantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) throw new ArgumentException("Product not found.");
            if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.");

            product.StockQuantity += quantity; // Add stock
            product.IsAvailable = product.StockQuantity > 0;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task AddProductPhotoAsync(IFormFile file, Product product)
        {
            var fileData = await FileHelper.ReadFileAsync(file);

            var productPhoto = new ProductPhoto
            {
                Photo = fileData,
                ProductId = product.Id
            };

            product.ProductPhotos.Add(productPhoto);
            await _context.SaveChangesAsync();
        }
    }
}
