namespace LoggerService.Exceptions.InternalError.Review;

public class UpdateReviewException : InternalServiceError
{
    public UpdateReviewException() 
        : base("Something went wrong while updating the review. Please try again later.")
    {
    }
}
