namespace LoggerService.Exceptions.InternalError.Users;

public class LoginFailedException : InternalServiceError
{
    public LoginFailedException() 
        : base("An error occurred while logging in. Please try again later.")
    {
    }
}
