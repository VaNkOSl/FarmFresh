using FarmFresh.ViewModels.Order;

namespace FarmFresh.Services.Contacts;

public interface ICartService
{
    Task<bool> AddToCartAsync(Guid userId, Guid productId, int quantity, bool trackChanges);

    Task RemoveFromCart(Guid productId, bool trackChanges);

    Task<bool> UpdateCartQuantityAsync(Guid productId, int quantityChange, bool trackChanges);

    Task<IEnumerable<CartItemViewModel>> GetAllCartItemsAsync(Guid userId, bool trackChanges);

    Task<decimal> GetTotalSumAsync(Guid userId, bool trackChanges);
}
