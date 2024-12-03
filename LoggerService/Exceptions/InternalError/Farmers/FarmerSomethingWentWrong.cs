namespace LoggerService.Exceptions.InternalError.Farmers;

public class FarmerSomethingWentWrong : InternalServiceError
{
    public FarmerSomethingWentWrong()
        : base("Something went wrong while creating the farmer. Please try again later.")
    {
    }
}
