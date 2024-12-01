namespace LoggerService.Exceptions.NotFound;

public class FarmerIdNotFoundException : NotFoundException
{
    public FarmerIdNotFoundException(Guid farmerId) 
        : base($"Farmer with the provided Id {farmerId} was not found. Please try again later or contact the administrator.")
    {
    }
}
