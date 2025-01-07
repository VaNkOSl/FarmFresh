using FarmFresh.ViewModels.Farmer;

namespace FarmFresh.Services.Contacts.FarmersInterfaces;

public interface IFarmerManagementService
{
    Task CreateFarmerAsync(FarmerForCreationDto model, string userId, bool trackChanges);

    Task DeleteFarmerAsync(Guid farmerId, bool trackChanges);

    Task EditFarmerAsync(FarmerForUpdatingDto model, Guid farmerId, bool trackChanges);
}
