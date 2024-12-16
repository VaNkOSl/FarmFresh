using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Repositories
{
    internal sealed class OrderRepository (FarmFreshDbContext data, IValidateEntity validateEntity) :
    RepositoryBase<Farmer>(data), IOrderRepository
    {
     

        public async Task AddOrderAsync(Order order)
        {
            data.Orders.Add(order);
            await data.SaveChangesAsync();
        }
        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await data.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task UpdateOrder(Order order)
        {
            data.Orders.Update(order);
            await data.SaveChangesAsync();
        }
        public async Task<Order> GetOrderWithDetailsAsync(Guid orderId)
        {
            return await data.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task UpdateOrderAsync(Order order)
        {
            data.Orders.Update(order);
            await data.SaveChangesAsync();
        }

        public async Task<List<OrderProduct>> GetOrderProductsByUserIdAsync(Guid userId)
        {
            return await data.OrderProducts
                .Include(o => o.Product)
                .Include(o => o.Order)
                .ThenInclude(o => o.User)
                .Where(o => o.Order.UserId == userId)
                .ToListAsync();
        }
        public async Task<OrderProduct> GetOrderProductDetailsByIdAsync(Guid id)
        {
            return await data.OrderProducts
                .Include(o => o.Product)
                    .ThenInclude(p => p.Farmer)
                    .ThenInclude(f => f.User)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AddOrderProductAsync(OrderProduct orderProduct)
        {
            data.OrderProducts.Add(orderProduct);
            await data.SaveChangesAsync();
        }
    }
}
