namespace LoggerService.Exceptions.BadRequest;

public class PhoneNumberAlreadyExistsException : BadRequestException
{
    public PhoneNumberAlreadyExistsException()
        : base("The phone number is already in use.")
    {
    }
}
