namespace LoggerService.Exceptions;

public abstract class InternalServiceError : Exception
{
    protected InternalServiceError(string messege)
        : base(messege)
    {
    }
}
