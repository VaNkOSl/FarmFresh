using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;

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
}
