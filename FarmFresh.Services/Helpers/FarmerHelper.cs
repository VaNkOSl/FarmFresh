using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;

namespace FarmFresh.Services.Helpers;

public static class FarmerHelper
{
    public static void ChekFarmerNotFound(
        object farmer,
        Guid farmerId,
        string methodName,
        ILoggerManager _loggerManager)
    {
        if (farmer is null)
        {
            _loggerManager.LogError($"[{nameof(methodName)}] Farmer with user ID {farmerId} not found.");
            throw new FarmerIdNotFoundException(farmerId);
        }
    }
}
