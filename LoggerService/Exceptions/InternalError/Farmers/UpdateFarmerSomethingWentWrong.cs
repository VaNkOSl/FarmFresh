﻿namespace LoggerService.Exceptions.InternalError.Farmers;

public class UpdateFarmerSomethingWentWrong : InternalServiceError
{
    public UpdateFarmerSomethingWentWrong()
        : base("An error occurred while updating the farmer. Please try again later.")
    {
    }
}
