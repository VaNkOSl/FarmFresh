namespace LoggerService.Exceptions.NotFound;

public class UserIdNotFound : NotFoundException
{
    public UserIdNotFound() 
        : base("User with the provided ID was not found. Please try again later or contact the administrator.")
    {
    }
}
