namespace LoggerService.Exceptions.InternalError.Categories;

public class DeleteCategorySomethingWentWrong : InternalServiceError
{
    public DeleteCategorySomethingWentWrong()
        : base("Something went wrong while deleting the category! Please try again later")
    {
    }
}
