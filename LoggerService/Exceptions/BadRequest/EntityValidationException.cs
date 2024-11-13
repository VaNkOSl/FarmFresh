namespace LoggerService.Exceptions.BadRequest;

public class EntityValidationException : BadRequestException
{
    public EntityValidationException() 
        : base("Validation failed for the provided data.")
    {
    }
}
