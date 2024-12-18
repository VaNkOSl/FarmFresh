using AutoMapper;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Order;
using LoggerService.Contacts;
using LoggerService.Exceptions.InternalError.CartItems;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services;

internal class CartService : ICartService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public CartService(IRepositoryManager repositoryManager,
                        ILoggerManager loggerManager,
                        IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task<bool> AddToCartAsync(Guid userId, Guid productId, int quantity, bool trackChanges)
    {
        try
        {
            var order = await CartHelper.EnsureOrderExistsAsync(userId, trackChanges, _repositoryManager, _loggerManager);

            await CartHelper.EnsureCartItemExistsAsync(userId, productId, quantity, trackChanges, _repositoryManager, _loggerManager);

            await CartHelper.AddOrUpdateOrderProductAsync(order.Id, productId, quantity, trackChanges, _repositoryManager, _loggerManager);

            _loggerManager.LogInfo($"[{nameof(AddToCartAsync)}] User with ID {userId} successfully add product with ID {productId} to cart!");
            return true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new AddItemToCartSomethingWentWrong();
        } 
    }

    public async Task<IEnumerable<CartItemViewModel>> GetAllCartItemsAsync(Guid userId, bool trackChanges)
    {
        var cartItems = await _repositoryManager.CartItemRepository
            .FindCartItemsByConditionAsync(ci => ci.UserId == userId, trackChanges)
            .GetCartItemsWithProductDetails()
            .ToListAsync();

        return _mapper.Map<IEnumerable<CartItemViewModel>>(cartItems);
    }


    public async Task RemoveFromCart(Guid productId, bool trackChanges)
    {
        var cartItem = await CartHelper.FindCartItemByProductId(productId, trackChanges, _repositoryManager, _loggerManager);
        var orderProductToRemove = await CartHelper.FindOrderProductByProductId(productId, trackChanges, _repositoryManager, _loggerManager);
        var order = await CartHelper.FindOrderByUserId(orderProductToRemove.Order.UserId, trackChanges, _repositoryManager, _loggerManager);

        try
        {
            _repositoryManager.CartItemRepository.DeleteItem(cartItem);
            _loggerManager.LogInfo($"[{nameof(RemoveFromCart)}] Successfully remove from cart item with ID {productId}");
            _repositoryManager.OrderProductRepository.DeleteOrderProduct(orderProductToRemove);
            _loggerManager.LogInfo($"[{nameof(RemoveFromCart)}] Successfully remove order product with ID {orderProductToRemove.Id}");
            _repositoryManager.OrderRepository.DeleteOrder(order);
            _loggerManager.LogInfo($"[{nameof(RemoveFromCart)}] Successfully remove order with ID {order.Id}");
            await _repositoryManager.SaveAsync();
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new RemoveFromCartSomethingWentWrong();
        }
    }

    public async Task<bool> UpdateCartQuantityAsync(Guid productId, int quantityChange, bool trackChanges)
    {
        var cart = await _repositoryManager.CartItemRepository
            .FindCartItemsByConditionAsync(ci => ci.ProductId == productId, trackChanges)
            .Include(p => p.Product)
            .FirstOrDefaultAsync();

        int newQuantity = cart.Quantity + quantityChange;

        if(newQuantity > cart.Product.StockQuantity || newQuantity < 1)
        {
            return false;
        }

        try
        {
            cart.Quantity = newQuantity;
            _repositoryManager.CartItemRepository.UpdateItem(cart);
            await _repositoryManager.SaveAsync();
            _loggerManager.LogInfo($"[{nameof(UpdateCartQuantityAsync)}] Quantity in the cart with ID {cart.Id} has been successfully updated.");
            return true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new UpdateQuantityFromCartSomethingWentWrong();
        }
    }
}