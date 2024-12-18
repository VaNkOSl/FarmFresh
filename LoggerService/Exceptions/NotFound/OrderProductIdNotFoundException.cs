namespace LoggerService.Exceptions.NotFound;

public class OrderProductIdNotFoundException : NotFoundException
{
    public OrderProductIdNotFoundException(Guid orderProductId)
       : base($"Order Product with ID '{orderProductId}' was not found. Please verify the database records.")
    {
    }
}
