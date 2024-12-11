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
        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task<Order> GetOrderWithDetailsAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderProduct>> GetOrderProductsByUserIdAsync(Guid userId)
        {
            return await _context.OrderProducts
                .Include(o => o.Product)
                .Include(o => o.Order)
                .ThenInclude(o => o.User)
                .Where(o => o.Order.UserId == userId)
                .ToListAsync();
        }
        public async Task<OrderProduct> GetOrderProductDetailsByIdAsync(Guid id)
        {
            return await _context.OrderProducts
                .Include(o => o.Product)
                    .ThenInclude(p => p.Farmer)
                    .ThenInclude(f => f.User)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
