namespace LoggerService.Exceptions.InternalError.ProductPhotos;

public class DeleteProductPhotosException : InternalServiceError
{
    public DeleteProductPhotosException(Guid productId) 
        : base($"Something went wrong while deleting the photos for the product with ID {productId}. Please try again later.")
    {
    }
}
