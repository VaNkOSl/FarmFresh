namespace LoggerService.Exceptions.InternalError.Users;

public class UpdateUserException : InternalServiceError
{
    public UpdateUserException() 
        : base("An error occurred while updating the user. Please try again later")
    {
    }
}
