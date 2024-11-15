namespace LoggerService.Exceptions.BadRequest;

public class EgnAlreadyExistsException : BadRequestException
{
    public EgnAlreadyExistsException() 
        : base("The EGN is already in use.")
    {
    }
}
