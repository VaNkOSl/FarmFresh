using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories;

public interface IOrderRepository
{
    Task<Order> AddOrderAsync(Order order);

    void DeleteOrder(Order order);

    void UpdateOrder(Order order);

    IQueryable<Order> FindOrderByConditionAsync(Expression<Func<Order, bool>> condition, bool trackChanges);

    IQueryable<Order> FindAllOrders(bool trackChanges);
}
