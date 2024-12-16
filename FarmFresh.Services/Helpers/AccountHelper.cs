using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;

namespace FarmFresh.Services.Helpers;

public static class AccountHelper
{
    public static void ChekIfUserIsNull(
        object user,
        Guid userId,
        string methodName,
        ILoggerManager _loggerManager)
    {
        if (user is null)
        {
            _loggerManager.LogError($"[{nameof(methodName)}] User with Id {userId} was not found!");
            throw new UserIdNotFoundException(userId);
        }
    }
}
