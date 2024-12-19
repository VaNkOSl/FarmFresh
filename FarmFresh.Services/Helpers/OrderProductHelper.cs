using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;

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
}
