namespace LoggerService.Exceptions.InternalError.Categories;

public class CategorySomethingWentWrong : InternalServiceError
{
    public CategorySomethingWentWrong()
        : base("Something went wrong while creating the category! Please try again later")
    {
    }
}
