namespace LoggerService.Exceptions.InternalError.Product;

public class DeleteProductException : InternalServiceError
{
    public DeleteProductException() 
        : base("Something went wrong while deleting the product. Please try again later.")
    {
    }
}
