using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories;

public interface ICartItemRepository
{
    Task<CartItem> CreateItemAsync(CartItem item);

    void DeleteItem(CartItem item);

    void UpdateItem(CartItem item);

    IQueryable<CartItem> FindCartItemsByConditionAsync(Expression<Func<CartItem, bool>> expression, bool trackChanges);
}
