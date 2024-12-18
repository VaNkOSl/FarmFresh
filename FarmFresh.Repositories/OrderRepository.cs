using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using System.Linq.Expressions;

namespace FarmFresh.Repositories;

internal class OrderRepository(FarmFreshDbContext data, IValidateEntity validateEntity)
    : RepositoryBase<Order>(data), IOrderRepository
{
    public async Task<Order> AddOrderAsync(Order order)
    {
        await CreateAsync(order);
        return order;
    }

    public void DeleteOrder(Order order) => Delete(order);

    public void UpdateOrder(Order order) => Update(order);

    public IQueryable<Order> FindOrderByConditionAsync(Expression<Func<Order, bool>> condition, bool trackChanges) =>
        FindByCondition(condition, trackChanges);

    public IQueryable<Order> FindAllOrders(bool trackChanges) =>
        FindAll(trackChanges);
}
