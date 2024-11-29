namespace LoggerService.Exceptions.NotFound;

public class UserIdNotFoundException : NotFoundException
{
    public UserIdNotFoundException() 
        : base("User with the provided ID was not found. Please try again later or contact the administrator.")
    {
    }
}
