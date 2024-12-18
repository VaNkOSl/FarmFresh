namespace LoggerService.Exceptions.InternalError.Review;

public class DeleteReviewException : InternalServiceError
{
    public DeleteReviewException() 
        : base("Something went wrong while deleting the product. Please try again later.")
    {
    }
}
