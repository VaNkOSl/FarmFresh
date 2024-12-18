using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories.DataValidator;
using System.Linq.Expressions;

namespace FarmFresh.Repositories;

internal class OrderProductRepository(FarmFreshDbContext data, IValidateEntity validateEntity)
    : RepositoryBase<OrderProduct>(data), IOrderProductRepository
{
    public async Task<OrderProduct> CreateOrderProductAsync(OrderProduct orderProduct)
    {
        await CreateAsync(orderProduct);
        return orderProduct;
    }

    public void DeleteOrderProduct(OrderProduct orderProduct) => Delete(orderProduct);

    public IQueryable<OrderProduct> FindAllOrderProducts(bool trackChanges) =>
        FindAll(trackChanges);

    public IQueryable<OrderProduct> FindOrderProductByConditionAsync(Expression<Func<OrderProduct, bool>> condition, bool trackChanges) =>
        FindByCondition(condition,trackChanges);
}
