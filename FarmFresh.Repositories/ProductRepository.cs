using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly FarmFreshDbContext _context;

    public ProductRepository(FarmFreshDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.Include(p => p.Category)
                                      .Include(p => p.Farmer)
                                      .Include(p => p.ProductPhotos)
                                      .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _context.Products.Include(p => p.Category)
                                      .Include(p => p.Farmer)
                                      .Include(p => p.ProductPhotos)
                                      .FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Product>> GetPagedProductsAsync(int pageIndex, int pageSize, string? filter)
    {
        return await _context.Products
                             .Where(p => string.IsNullOrEmpty(filter) || p.Name.Contains(filter))
                             .OrderBy(p => p.Name)
                             .Skip((pageIndex - 1) * pageSize)
                             .Take(pageSize)
                             .Include(p => p.Category)
                             .Include(p => p.Farmer)
                             .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? filter)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(p => p.Name.Contains(filter));
        }

        return await query.CountAsync();
    }
}
