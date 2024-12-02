using FarmFresh.Data.Models;
using FarmFresh.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FarmFresh.Data.Models.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly FarmFreshDbContext _context;

    public CategoryRepository(FarmFreshDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        // Fetch all categories from the database
        return await _context.Categories.AsNoTracking().ToListAsync();
    }
}
