namespace LoggerService.Exceptions.NotFound;

public class OrderIdNotFoundException : NotFoundException
{
    public OrderIdNotFoundException(Guid orderId)
   : base($"Order with ID '{orderId}' was not found. Please verify the database records.")
    {
    }
}
