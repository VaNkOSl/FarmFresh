namespace LoggerService.Exceptions.InternalError.CartItems;

public class RemoveFromCartSomethingWentWrong : InternalServiceError
{
    public RemoveFromCartSomethingWentWrong() 
        : base("Something went wrong while removing product from cart. Please try again later.")
    {
    }
}
