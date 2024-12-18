namespace LoggerService.Exceptions.InternalError.Users;

public class DeleteUserSomethingWentWrong : InternalServiceError
{
    public DeleteUserSomethingWentWrong() 
        : base("An error occurred while deleting the user. Please try again later.")
    {
    }
}
