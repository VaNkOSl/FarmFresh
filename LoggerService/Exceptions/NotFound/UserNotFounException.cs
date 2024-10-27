namespace LoggerService.Exceptions.NotFound;

public class UserNotFounException : NotFoundException
{
    public UserNotFounException()
        : base("User with provided email or username does not exist!")
    {
    }
}
