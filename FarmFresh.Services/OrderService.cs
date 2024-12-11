using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Data.Models;
using FarmFresh.Services.Contacts;

namespace FarmFresh.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Guid> CheckoutAsync(Order order, Guid orderProductId, Guid userId)
        {
            order.UserId = userId;
            order.OrderStatus = OrderStatus.Cart;
            order.CreateOrderdDate = DateTime.Now;

            await _orderRepository.AddOrderAsync(order);
            return order.Id;
        }
    }
}
