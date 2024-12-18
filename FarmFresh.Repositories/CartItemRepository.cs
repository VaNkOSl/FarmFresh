using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using System.Linq.Expressions;

namespace FarmFresh.Repositories;

internal sealed class CartItemRepository(FarmFreshDbContext data, IValidateEntity validateEntity)
    : RepositoryBase<CartItem>(data), ICartItemRepository
{
    public async Task<CartItem> CreateItemAsync(CartItem item)
    {
        await CreateAsync(item);
        return item;
    }

    public void DeleteItem(CartItem item) => Delete(item);

    public void UpdateItem(CartItem item) => Update(item);

    public IQueryable<CartItem> FindCartItemsByConditionAsync(Expression<Func<CartItem, bool>> expression, bool trackChanges) =>
        FindByCondition(expression, trackChanges);
}
