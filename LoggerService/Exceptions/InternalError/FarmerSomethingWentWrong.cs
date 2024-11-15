namespace LoggerService.Exceptions.InternalError;

public class FarmerSomethingWentWrong : InternalServiceError
{
    public FarmerSomethingWentWrong() 
        : base("Something went wrong while creating the farmer. Please try again later.")
    {
    }
}
