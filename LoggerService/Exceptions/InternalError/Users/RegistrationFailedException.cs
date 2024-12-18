namespace LoggerService.Exceptions.InternalError.Users;

public class RegistrationFailedException : InternalServiceError
{
    public RegistrationFailedException() 
        : base("An error occurred while registering the user. Please try again later.")
    {
    }
}
