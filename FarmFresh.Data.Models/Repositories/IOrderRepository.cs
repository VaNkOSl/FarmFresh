using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.Models.Repositories
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);

        Task UpdateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<Order> GetOrderWithDetailsAsync(Guid orderId);
        Task<List<OrderProduct>> GetOrderProductsByUserIdAsync(Guid userId);
        Task<OrderProduct> GetOrderProductDetailsByIdAsync(Guid id);
    }
}
