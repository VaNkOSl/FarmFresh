namespace LoggerService.Exceptions.NotFound;

public class ReviewIdNotFoundException : NotFoundException
{
    public ReviewIdNotFoundException(Guid reviewId) 
        : base($"Review with the provided Id {reviewId} was not found. Please try again later or contact the administrator.")
    {
    }
}
