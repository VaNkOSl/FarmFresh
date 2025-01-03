using AutoMapper;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Order;
using LoggerService.Contacts;
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
        var order = await CartHelper.EnsureOrderExistsAsync(userId, trackChanges, _repositoryManager);

        await CartHelper.EnsureCartItemExistsAsync(userId, productId, quantity, trackChanges, _repositoryManager);

        await CartHelper.AddOrUpdateOrderProductAsync(order.Id, productId, quantity, trackChanges, _repositoryManager);

        return true;
    }

    public async Task<IEnumerable<CartItemViewModel>> GetAllCartItemsAsync(Guid userId, bool trackChanges)
    {
        var cartItems = await _repositoryManager.CartItemRepository
            .FindCartItemsByConditionAsync(ci => ci.UserId == userId, trackChanges)
            .Include(p => p.Product)
            .ThenInclude(ph => ph.ProductPhotos)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CartItemViewModel>>(cartItems);
    }

    public async Task<decimal> GetTotalSumAsync(Guid userId, bool trackChanges)
    {
        var cartItems = await _repositoryManager.CartItemRepository
            .FindCartItemsByConditionAsync(ci => ci.UserId == userId, trackChanges)
            .GetCartItemsWithProductDetails()
            .ToListAsync();

        var totalSum = cartItems.Sum(ci => ci.Quantity * ci.Product.Price);

        return totalSum;
    }

    public async Task RemoveFromCart(Guid productId, bool trackChanges)
    {
        var cartItem = await _repositoryManager.CartItemRepository
            .FindCartItemsByConditionAsync(ci => ci.ProductId == productId, trackChanges)
            .FirstOrDefaultAsync();

<<<<<<< HEAD
        var orderProductToRemove = await _repositoryManager.OrderProductRepository
            .FindOrderProductByConditionAsync(oi => oi.ProductId == productId, trackChanges)
            .Include(o => o.Order)
            .FirstOrDefaultAsync();

        var order = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(o => o.UserId == orderProductToRemove.Order.UserId, trackChanges)
            .FirstOrDefaultAsync();

        _repositoryManager.CartItemRepository.DeleteItem(cartItem);
        _repositoryManager.OrderProductRepository.DeleteOrderProduct(orderProductToRemove);
        _repositoryManager.OrderRepository.DeleteOrder(order);
        await _repositoryManager.SaveAsync();
=======
        try
        {
            var productToUpdate = await _repositoryManager.ProductRepository
                .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
                .FirstOrDefaultAsync();

            if (productToUpdate != null)
            {
                productToUpdate.StockQuantity += orderProductToRemove.Quantity;

                _repositoryManager.ProductRepository.UpdateProduct(productToUpdate);
                _loggerManager.LogInfo($"[{nameof(RemoveFromCart)}] Successfully updated product quantity for product with ID {productId}");
            }
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
>>>>>>> development
    }

    public async Task<bool> UpdateCartQuantityAsync(Guid productId, int quantityChange, bool trackChanges)
    {
        var cart = await _repositoryManager.CartItemRepository
            .FindCartItemsByConditionAsync(ci => ci.ProductId == productId, trackChanges)
            .Include(p => p.Product)
            .FirstOrDefaultAsync();

        var product = await _repositoryManager.ProductRepository
            .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
            .FirstOrDefaultAsync();

        ProductHelper.CheckProductNotFound(product, productId, "UpdateCartQuantityAsync", _loggerManager);

        int currentCartQuantity = cart.Quantity;
        int newQuantity = currentCartQuantity + quantityChange;

        if(newQuantity > cart.Product.StockQuantity || newQuantity < 1)
        {
            return false;

        }

<<<<<<< HEAD
        cart.Quantity = newQuantity;
        _repositoryManager.CartItemRepository.UpdateItem(cart);
        await _repositoryManager.SaveAsync();
        return true;
=======
        try
        {
            int stockAdjustment = currentCartQuantity - newQuantity;
            product.StockQuantity += stockAdjustment;
            cart.Quantity = newQuantity;
            _repositoryManager.ProductRepository.UpdateProduct(product);
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
>>>>>>> development
    }
}