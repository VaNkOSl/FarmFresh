using FarmFresh.Commons.RequestFeatures;
using FarmFresh.ViewModels.Farmer;

namespace FarmFresh.Services.Contacts.FarmersInterfaces;

public interface IFarmerQueryService
{
    Task<FarmerProfileViewModel> GetFarmerProfileAsync(string userId);

    Task<FarmerDetailsDto> GetFarmersDetailsByIdAsync(Guid farmerId, bool trackChanges);

    Task<Guid> GetFarmerByUserIdAsync(Guid userId, bool trackChanges);

    Task<FarmerForUpdatingDto> GetFarmerForEditAsync(Guid farmerId, bool trackChanges);

    Task<(IEnumerable<FarmersViewModel> farmers, MetaData metaData)> GetAllFarmersAsync(FarmerParameters farmerParameters, bool trackChanges);

    Task<FarmersListViewModel> CreateFarmersListViewModelAsync(IEnumerable<FarmersViewModel> farmers, MetaData metaData, string? searchTerm);
}
