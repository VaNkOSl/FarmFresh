using FarmFresh.ViewModels.Farmer;

namespace FarmFresh.Services.Contacts;

public interface IFarmerService
{
    Task CreateFarmerAsync(FarmerCreateUpdateForm model, string userId, bool tracktrackChanges);

    Task CreateFarmerLocationAsync(FarmerLocationDto model, Guid farmerId);

    Task<bool> DoesFarmerExistAsync(string egn, string phoneNumber, string userId, bool tracktrackChanges);
}
