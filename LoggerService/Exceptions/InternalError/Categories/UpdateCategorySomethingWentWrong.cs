namespace LoggerService.Exceptions.InternalError.Categories;

public class UpdateCategorySomethingWentWrong : InternalServiceError
{
    public UpdateCategorySomethingWentWrong()
        : base("Something went wrong while updating the category! Please try again later")
    {
    }
}
