using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Repositories;
using FarmFresh.Repositories.Contacts;
using LoggerService;
using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services.Helpers;

public static class CartHelper
{
    public static async Task<Order> EnsureOrderExistsAsync(Guid userId, bool trackChanges, IRepositoryManager _repositoryManager, ILoggerManager _loggerManager)
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
            _loggerManager.LogInfo($"[{nameof(EnsureOrderExistsAsync)}] Successfully created order with ID {order.Id}");
        }

        return order;
    }

    public static async Task<CartItem> EnsureCartItemExistsAsync(Guid userId, Guid productId, int quantity, bool trackChanges, IRepositoryManager _repositoryManager, ILoggerManager _loggerManager)
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
        _loggerManager.LogInfo($"[{nameof(EnsureCartItemExistsAsync)}] Successfully add product with Id {productId} to cart item with Id {cartItem.Id}");
        return cartItem;
    }

    public static async Task<OrderProduct> AddOrUpdateOrderProductAsync(Guid orderId, Guid productId, int quantity, bool trackChanges, IRepositoryManager _repositoryManager, ILoggerManager _loggerManager)
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
        _loggerManager.LogInfo($"[{nameof(AddOrUpdateOrderProductAsync)}] Successfully created orderItem with Id {orderItem.Id}.");
        return orderItem;
    }

    public static void CheckCartItemNotFound(
        object cartItem,
        Guid cartItemId,
        string methodName,
        ILoggerManager _loggerManager)
    {
        if(cartItem is null)
        {
            _loggerManager.LogError($"[{methodName}] Cart with Id {cartItemId} was not found at Date: {DateTime.UtcNow}");
            throw new CartIdNotFoundException(cartItemId);
        }
    }

   public static async Task<CartItem> FindCartItemByProductId(Guid productId, bool trackChanges, IRepositoryManager _repositoryManager, ILoggerManager loggerManager)
    {
        var cartItem = await _repositoryManager.CartItemRepository
        .FindCartItemsByConditionAsync(ci => ci.ProductId == productId, trackChanges)
        .FirstOrDefaultAsync();

        CheckCartItemNotFound(cartItem, cartItem.Id, nameof(FindCartItemByProductId), loggerManager);
        return cartItem;
    }

    public static async Task<OrderProduct> FindOrderProductByProductId(Guid productId, bool trackChanges, IRepositoryManager _repositoryManager, ILoggerManager _loggerManager)
    {
        var orderProductToRemove = await _repositoryManager.OrderProductRepository
            .FindOrderProductByConditionAsync(oi => oi.ProductId == productId, trackChanges)
            .Include(o => o.Order)
            .FirstOrDefaultAsync();

        OrderProductHelper.CheckOrderProductNotFound(orderProductToRemove, orderProductToRemove.Id, nameof(FindOrderProductByProductId), _loggerManager);
        return orderProductToRemove;
    }

    public static async Task<Order> FindOrderByUserId(Guid userId, bool trackChanges, IRepositoryManager _repositoryManager, ILoggerManager _loggerManager)
    {
        var order = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(o => o.UserId == userId, trackChanges)
            .FirstOrDefaultAsync();

        OrderHelper.ChekOrderNotFound(order, order.Id, nameof(FindOrderByUserId), _loggerManager);
        return order;
    }
}
