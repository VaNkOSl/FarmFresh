namespace LoggerService.Exceptions.InternalError.Order;

public class OrderCheckoutInternalServiceError : InternalServiceError
{
    public OrderCheckoutInternalServiceError() 
        : base("Something went wrong while updating order. Please try again later.")
    {
    }
}
