using FarmFresh.ViewModels.Farmer;

namespace FarmFresh.Services.Contacts;

public interface IFarmerService
{
    Task CreateFarmerAsync(FarmerCreateUpdateForm model, string userId);

    Task<bool> FarmerWithProvidedUserIdAlreadyExistsAsync(string userId);

    Task<bool> FarmerEgnAlreadyExistsAsync(string egn);

    Task<bool> FarmerPhoneNumberAlreadyExists(string phoneNumber);
}
