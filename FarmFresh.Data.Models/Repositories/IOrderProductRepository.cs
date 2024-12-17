using System.Linq.Expressions;

namespace FarmFresh.Data.Models.Repositories;

public interface IOrderProductRepository
{
    Task<OrderProduct> CreateOrderProductAsync(OrderProduct orderProduct);

    void DeleteOrderProduct(OrderProduct orderProduct);

    IQueryable<OrderProduct> FindAllOrderProducts(bool trackChanges);

    IQueryable<OrderProduct> FindOrderProductByConditionAsync(Expression<Func<OrderProduct, bool>> condition, bool trackChanges);
}
