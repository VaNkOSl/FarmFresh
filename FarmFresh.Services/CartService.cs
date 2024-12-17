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


    public async Task RemoveFromCart(Guid productId, bool trackChanges)
    {
        var cartItem = await _repositoryManager.CartItemRepository
            .FindCartItemsByConditionAsync(ci => ci.ProductId == productId, trackChanges)
            .FirstOrDefaultAsync();

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

        cart.Quantity = newQuantity;
        _repositoryManager.CartItemRepository.UpdateItem(cart);
        await _repositoryManager.SaveAsync();
        return true;
    }
}