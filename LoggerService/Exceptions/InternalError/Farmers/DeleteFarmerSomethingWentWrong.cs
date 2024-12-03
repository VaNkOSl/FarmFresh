namespace LoggerService.Exceptions.InternalError.Farmers;

public class DeleteFarmerSomethingWentWrong : InternalServiceError
{
    public DeleteFarmerSomethingWentWrong()
        : base("Something went wrong while deleting the farmer. Please try again later.")
    {
    }
}
