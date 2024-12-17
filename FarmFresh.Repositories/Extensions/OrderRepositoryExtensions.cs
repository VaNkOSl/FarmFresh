using FarmFresh.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Repositories.Extensions;

public static class OrderRepositoryExtensions
{
    public static IQueryable<Order> GetOrderWithDetails(this IQueryable<Order> order) =>
           order.Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product);

    public static IQueryable<OrderProduct> GetOrderProductsByUserId(this IQueryable<Order> orders, Guid userId)
    {
        return orders
            .Where(o => o.UserId == userId)
            .SelectMany(o => o.OrderProducts) 
            .Include(op => op.Product) 
            .Include(op => op.Order) 
            .ThenInclude(o => o.User); 
    }

    public static IQueryable<OrderProduct> GetOrderProductDetailsById(this IQueryable<OrderProduct> orderProducts, Guid id)
    {
        return orderProducts
            .Include(o => o.Product)
            .ThenInclude(p => p.Farmer)
            .ThenInclude(f => f.User)
            .Include(o => o.Order)
            .ThenInclude(ph => ph.ProductPhotos)
            .Where(o => o.Id == id);
    }
}

