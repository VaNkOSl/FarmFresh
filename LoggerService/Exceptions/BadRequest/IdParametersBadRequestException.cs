namespace LoggerService.Exceptions.BadRequest;         

public class IdParametersBadRequestException : BadRequestException
{
    public IdParametersBadRequestException() 
        : base("Parameter Id's is null!")
    {
    }
}
