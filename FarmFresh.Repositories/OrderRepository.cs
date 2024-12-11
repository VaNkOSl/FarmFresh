using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;

namespace FarmFresh.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FarmFreshDbContext _context;

        public OrderRepository(FarmFreshDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}
