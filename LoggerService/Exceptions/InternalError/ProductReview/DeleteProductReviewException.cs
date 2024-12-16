namespace LoggerService.Exceptions.InternalError.ProductReview;

public class DeleteProductReviewException : InternalServiceError
{
    public DeleteProductReviewException(Guid productId) 
        : base($"Something went wrong while deleting the reviews for the product with ID {productId}. Please try again later.")
    {
    }
}
