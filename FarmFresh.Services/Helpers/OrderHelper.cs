using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;
using FarmFresh.Repositories.Extensions;
using FarmFresh.Data.Models.Repositories;

namespace FarmFresh.Services.Helpers;

public static class OrderHelper
{
    public static void ChekOrderNotFound(
        object order,
        Guid orderId,
        string methodName,
        ILoggerManager _loggerManager)
    {
        if (order is null)
        {
            _loggerManager.LogError($"[{methodName}] Order with Id {orderId} was not found at Date: {DateTime.UtcNow}");
            throw new OrderIdNotFoundException(orderId);
        }
    }

    public static async Task<Order> GetOrderByIdAndCheckIfExists(Guid id, bool trackChanges, IRepositoryManager _repositoryManager, ILoggerManager _loggerManager)
    {
        var order = await _repositoryManager.OrderRepository
            .FindOrderByConditionAsync(o => o.Id == id, trackChanges)
            .GetOrderWithDetails()
            .FirstOrDefaultAsync();

        if (order is null)
        {
            _loggerManager.LogError($"[{GetOrderByIdAndCheckIfExists}] Order with Id {id} was not found at Date: {DateTime.UtcNow}");
            throw new OrderIdNotFoundException(id);
        }

        return order;
    }
}
