namespace LoggerService.Exceptions;

public abstract class BadRequestException : Exception
{
    protected BadRequestException(string messege)
        : base(messege)
    {
    }
}