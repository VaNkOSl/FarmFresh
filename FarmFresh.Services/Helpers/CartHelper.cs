using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Repositories.Contacts;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services.Helpers;

public static class CartHelper
{
    public static async Task<Order> EnsureOrderExistsAsync(Guid userId, bool trackChanges, IRepositoryManager _repositoryManager)
    {
        var order = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(o => o.UserId == userId && o.OrderStatus == OrderStatus.Cart, trackChanges)
            .FirstOrDefaultAsync();

        if (order is null)
        {
            order = new Order
            {
                UserId = userId,
                OrderStatus = OrderStatus.Cart,
                CreateOrderdDate = DateTime.Now,
            };
            await _repositoryManager.OrderRepository.AddOrderAsync(order);
            await _repositoryManager.SaveAsync();
        }

        return order;
    }

    public static async Task<CartItem> EnsureCartItemExistsAsync(Guid userId, Guid productId, int quantity, bool trackChanges, IRepositoryManager _repositoryManager)
    {
        var cartItem = await _repositoryManager.CartItemRepository
             .FindCartItemsByConditionAsync(ci => ci.UserId == userId && ci.ProductId == productId, trackChanges)
             .FirstOrDefaultAsync();

        if (cartItem == null)
        {
            cartItem = new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
            };

            await _repositoryManager.CartItemRepository.CreateItemAsync(cartItem);
        }
        else
        {
            cartItem.Quantity += quantity;
        }

        await _repositoryManager.SaveAsync();
        return cartItem;
    }

    public static async Task<OrderProduct> AddOrUpdateOrderProductAsync(Guid orderId, Guid productId, int quantity, bool trackChanges, IRepositoryManager _repositoryManager)
    {
        var product = await _repositoryManager.ProductRepository
            .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
            .FirstOrDefaultAsync();

        if (product == null)
            throw new InvalidOperationException("Product not found.");

        var orderItem = await _repositoryManager.OrderProductRepository
            .FindOrderProductByConditionAsync(op => op.ProductId == productId && op.OrderId == orderId, trackChanges)
            .FirstOrDefaultAsync();

        if (orderItem == null)
        {
            orderItem = new OrderProduct
            {
                ProductId = productId,
                OrderId = orderId,
                Quantity = quantity,
                Price = product.Price,
            };

            await _repositoryManager.OrderProductRepository.CreateOrderProductAsync(orderItem);
        }
        else
        {
            orderItem.Quantity += quantity;
        }

        await _repositoryManager.SaveAsync();
        return orderItem;
    }
}
