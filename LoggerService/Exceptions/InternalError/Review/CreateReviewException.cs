namespace LoggerService.Exceptions.InternalError.Review;

public class CreateReviewException : InternalServiceError
{
    public CreateReviewException() 
        : base("Something went wrong while creating the review. Please try again later.")
    {
    }
}
