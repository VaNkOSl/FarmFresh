using FarmFresh.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Repositories.Extensions;

public static class CartItemRepositoryExtensions
{
    public static IQueryable<CartItem> GetCartItemsWithProductDetails(this IQueryable<CartItem> cartItem) =>
          cartItem
            .Include(ci => ci.Product)
            .ThenInclude(ph => ph.ProductPhotos);
}
