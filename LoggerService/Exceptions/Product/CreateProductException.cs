namespace LoggerService.Exceptions.Product;

public class CreateProductException : InternalServiceError
{
    public CreateProductException() 
        : base("Something went wrong while creating the product. Please try again later.")
    {
    }
}
