using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Data.Models;
using FarmFresh.Services.Contacts;
using Microsoft.AspNetCore.Http;
using FarmFresh.ViewModels.Order;
using System.Text.Json;

namespace FarmFresh.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderService(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> CheckoutAsync(Order order, Guid orderProductId, Guid userId)
        {
            order.UserId = userId;
            order.OrderStatus = OrderStatus.Cart; 
            order.CreateOrderdDate = DateTime.Now; 
            var cartItems = _httpContextAccessor.HttpContext.Session.Get<List<CartItemViewModel>>("Cart");

            if (cartItems == null || !cartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty.");
            }
            await _orderRepository.AddOrderAsync(order);

            foreach (var cartItem in cartItems)
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price
                };

                await _orderRepository.AddOrderProductAsync(orderProduct);
            }

            _httpContextAccessor.HttpContext.Session.Remove("Cart");

            return order.Id;
        }
        public async Task CompleteOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            order.OrderStatus = OrderStatus.Completed;
            await _orderRepository.UpdateOrderAsync(order);
        }
        public async Task<OrderConfirmationViewModel> GetOrderConfirmationViewModelAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            return new OrderConfirmationViewModel
            {
                Id = order.Id,
                OrderId = order.Id,
                Products = order.OrderProducts.ToList(),
                Price = order.OrderProducts.Sum(p => p.Price),
                Quantity = order.OrderProducts.Sum(p => p.Quantity),
                TotalPrice = order.OrderProducts.Sum(p => p.Price * p.Quantity),
                Picture = order.OrderProducts.FirstOrDefault()?.Product?.Photo,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Adress = order.Adress,
                PhoneNumber = order.PhoneNumber,
                Email = order.Email,
            };
        }
        public async Task<List<OrderListViewModel>> GetOrdersForUserAsync(Guid userId)
        {
            var orderProducts = await _orderRepository.GetOrderProductsByUserIdAsync(userId);

            return orderProducts.Select(o => new OrderListViewModel
            {
                Id = o.Id,
                OrderId = o.OrderId,
                ProductName = o.Product.Name,
                OrderStatus = o.Order.OrderStatus.ToString(),
                Price = o.Price,
                Quantity = o.Quantity,
                Picture = o.Product.Photo
            }).ToList();
        }
        public async Task<OrderDetailsViewModel> GetOrderDetailsAsync(Guid id)
        {
            var orderProduct = await _orderRepository.GetOrderProductDetailsByIdAsync(id);
            if (orderProduct == null)
            {
                return null;
            }

            return new OrderDetailsViewModel
            {
                Id = orderProduct.OrderId,
                CreatedDate = orderProduct.Order.CreateOrderdDate,
                OrderId = orderProduct.OrderId,
                Quantity = orderProduct.Quantity,
                Price = orderProduct.Price,
                FirstName = orderProduct.Order.FirstName,
                LastName = orderProduct.Order.LastName,
                Adress = orderProduct.Order.Adress,
                PhoneNumber = orderProduct.Order.PhoneNumber,
                Email = orderProduct.Order.Email,
                ProductName = orderProduct.Product.Name,
                DeliveryOption = orderProduct.Order.DeliveryOption,
                OrderStatus = orderProduct.Order.OrderStatus.ToString(),
                ProductDescription = orderProduct.Product.Description,
                FarmerName = $"{orderProduct.Product.Farmer.User.FirstName} {orderProduct.Product.Farmer.User.LastName}",
                Origin = orderProduct.Product.Origin,
                ProductPrice = orderProduct.Product.Price,
                Seasons = orderProduct.Product.SuitableSeason,
                HarvestDate = orderProduct.Product.HarvestDate,
                ExpirationDate = orderProduct.Product.ExpirationDate,
                Picture = orderProduct.Product.Photo
            };
        }
    }
    public static class SessionExtension
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

    }
}
