namespace LoggerService.Exceptions.InternalError.Product;

public class UpdateProductException : InternalServiceError
{
    public UpdateProductException() 
        : base("Something went wrong while updating the product. Please try again later.")
    {
    }
}
