namespace LoggerService.Exceptions.NotFound;

public class ProductIdNotFoundException : NotFoundException
{
    public ProductIdNotFoundException(Guid productId) 
        : base($"Product with ID {productId} was not found. Please verify the database records.")
    {
    }
}
