namespace LoggerService.Exceptions.BadRequest;

public class UserIdAlreadyExistsException : BadRequestException
{
    public UserIdAlreadyExistsException() 
        : base("The userId is already in use.")
    {
    }
}
