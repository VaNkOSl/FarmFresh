using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Repositories.Extensions;

public static class OrderRepositoryExtensions
{
    public static IQueryable<Order> GetOrderWithDetails(this IQueryable<Order> order) =>
           order.Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ThenInclude(ph => ph.ProductPhotos);

    public static IQueryable<OrderProduct> GetOrderProductsByUserId(this IQueryable<Order> orders, string userId)
    {
        return orders
            .Where(o => o.UserId.ToString() == userId)
            .SelectMany(o => o.OrderProducts) 
            .Include(op => op.Product)
            .ThenInclude(ph => ph.ProductPhotos)
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

    public static IQueryable<OrderProduct> GetOrderProductsByFarmerId(this IQueryable<Order> orders, Guid farmerId)
    {
        return orders
            .SelectMany(o => o.OrderProducts)
            .Where(op => op.Product.FarmerId == farmerId &&
            op.Order.OrderStatus != OrderStatus.Shipped 
            && op.Order.OrderStatus != OrderStatus.ReadyForPickup)
            .Include(o => o.Order)
            .ThenInclude(ph => ph.ProductPhotos)
            .Include(ph => ph.Product)
            .ThenInclude(f => f.Farmer)
            .ThenInclude(u => u.User);
    }

    public static IQueryable<Order> GetAllProductsForReviewByUserAsync(this IQueryable<Order> orders) =>
        orders.Include(o => o.OrderProducts)
         .ThenInclude(op => op.Product)
         .ThenInclude(p => p.ProductPhotos)
         .Include(o => o.OrderProducts)
         .ThenInclude(op => op.Product)
         .ThenInclude(p => p.Reviews);
}

