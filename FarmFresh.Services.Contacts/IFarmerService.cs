using FarmFresh.Commons.RequestFeatures;
using FarmFresh.ViewModels.Farmer;

namespace FarmFresh.Services.Contacts;

public interface IFarmerService
{
    Task CreateFarmerAsync(FarmerForCreationDto model, string userId, bool trackChanges);

    Task<bool> DoesFarmerExistAsync(string egn, string phoneNumber, string userId, bool trackChanges);

    Task<(IEnumerable<FarmersViewModel> farmers, MetaData metaData)> GetAllFarmersAsync(FarmerParameters farmerParameters, bool trackChanges);

    Task<FarmersListViewModel> CreateFarmersListViewModelAsync(IEnumerable<FarmersViewModel> farmers, MetaData metaData, string? searchTerm);

    Task<bool> DoesFarmerExistsByuserId(string userId, bool trackChanges);

    Task DeleteFarmerAsync(Guid farmerId, bool trackChanges);

    Task<FarmerForUpdatingDto> GetFarmerForEditAsync(Guid farmerId, bool trackChanges);

    Task EditFarmerAsync(FarmerForUpdatingDto model, Guid farmerId, bool trackChanges);

    Task<FarmerProfileViewModel> GetFarmerProfileAsync(string userId);

    Task<bool> DoesFarmerHasProductsAsync(string userId, Guid productId ,bool trackChanges);
}
