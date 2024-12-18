namespace LoggerService.Exceptions.NotFound;

public class UserIdNotFoundException : NotFoundException
{
    public UserIdNotFoundException(Guid userId) 
        : base($"User with the provided Id {userId} was not found. Please try again later or contact the administrator.")
    {
    }
}
