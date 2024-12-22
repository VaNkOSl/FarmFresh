using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;
using FarmFresh.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services.Helpers;

public static class OrderProductHelper
{
    public static void CheckOrderProductNotFound(
      object orderProduct,
      Guid orderProductId,
      string methodName,
      ILoggerManager _loggerManager)
    {
        if (orderProduct is null)
        {
            _loggerManager.LogError($"[{methodName}] OrderProduct with Id {orderProductId} was not found at Date: {DateTime.UtcNow}");
            throw new OrderProductIdNotFoundException(orderProductId);
        }
    }

    public static async Task<OrderProduct> GetOrderProductAndCheckIfItExists(Guid id, bool trackChanges, IRepositoryManager _repositoryManager, ILoggerManager _loggerManager)
    {
        var orderProduct = await _repositoryManager.OrderProductRepository
                      .FindAllOrderProducts(trackChanges)
                      .GetOrderProductDetailsById(id)
                      .Include(p => p.Product)
                      .ThenInclude(ph => ph.ProductPhotos)
                      .FirstOrDefaultAsync();

        if (orderProduct is null)
        {
            _loggerManager.LogError($"OrderProduct with Id {id} was not found at {DateTime.UtcNow}");
            throw new OrderProductIdNotFoundException(id);
        }

        return orderProduct;
    }
}
