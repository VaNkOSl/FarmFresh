using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly FarmFreshDbContext _context;

        public ProductRepository(FarmFreshDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }
    }
}
