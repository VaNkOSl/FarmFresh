namespace LoggerService.Exceptions.InternalError.CartItems;

public class UpdateQuantityFromCartSomethingWentWrong : InternalServiceError
{
    public UpdateQuantityFromCartSomethingWentWrong()
       : base("Something went wrong while updating the quantity of the product from cart. Please try again later.")
    {
    }
}
