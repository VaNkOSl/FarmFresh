namespace LoggerService.Exceptions.InternalError.CartItems;

public class AddItemToCartSomethingWentWrong : InternalServiceError
{
    public AddItemToCartSomethingWentWrong() 
        : base("Something went wrong while adding item to cart. Please try again later.")
    {
    }
}
