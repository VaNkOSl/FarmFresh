namespace LoggerService.Exceptions.BadRequest;

public class InvalidUserIdFormatException : BadRequestException
{
    public InvalidUserIdFormatException() 
        : base("The userId format is invalid. Please provide a valid GUID.")
    {
    }
}
