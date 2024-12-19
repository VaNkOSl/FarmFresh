namespace LoggerService.Exceptions.NotFound;

public class CartIdNotFoundException : NotFoundException
{
    public CartIdNotFoundException(Guid cartId) 
        : base($"Cart with ID '{cartId}' was not found. Please verify the database records.")
    {
    }
}
