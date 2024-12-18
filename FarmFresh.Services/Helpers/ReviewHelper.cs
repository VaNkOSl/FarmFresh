using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;

namespace FarmFresh.Services.Helpers;

public static class ReviewHelper
{
    public static void CheckIfReviewIsNull(
      object review,
      Guid reviewId,
      string methodName,
      ILoggerManager _loggerManager)
    {
        if (review is null)
        {
            _loggerManager.LogError($"[{methodName}] Review with Id {reviewId} was not found at Date: {DateTime.UtcNow}");
            throw new ReviewIdNotFoundException(reviewId);
        }
    }
}
