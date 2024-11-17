namespace LoggerService.Exceptions.BadRequest;

public class FarmerAlreadyExistsException : BadRequestException
{
    public FarmerAlreadyExistsException() 
        : base("Farmer with EGN, phone number or userId already exists.")
    {
    }
}
