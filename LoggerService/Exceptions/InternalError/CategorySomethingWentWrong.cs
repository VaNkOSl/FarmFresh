namespace LoggerService.Exceptions.InternalError;

public class CategorySomethingWentWrong : InternalServiceError
{
    public CategorySomethingWentWrong() 
        : base("Something went wrong while creating the category! Plse try againg later")
    {
    }
}
