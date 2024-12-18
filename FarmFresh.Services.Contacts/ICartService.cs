using FarmFresh.ViewModels.Order;
using Microsoft.AspNetCore.Http;

namespace FarmFresh.Services.Contacts;

public interface ICartService
{
    Task<bool> AddToCartAsync(Guid userId, Guid productId, int quantity, bool trackChanges);
    // Task<bool> AddToCartAsync(Guid userId, Guid productId, string sessionKey, ISession session, bool trackChanges);
    Task RemoveFromCart(Guid productId, bool trackChanges);
    Task<bool> UpdateCartQuantityAsync(Guid productId, int quantityChange, bool trackChanges);

    Task<IEnumerable<CartItemViewModel>> GetAllCartItemsAsync(Guid userId, bool trackChanges);
}
