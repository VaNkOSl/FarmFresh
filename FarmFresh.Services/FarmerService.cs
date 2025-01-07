using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Contacts.FarmersInterfaces;
using FarmFresh.ViewModels.Farmer;

namespace FarmFresh.Services;

internal sealed class FarmerService : IFarmerService
{
    private readonly IFarmerValidationService _farmerValidationService;
    private readonly IFarmerManagementService _farmerManagementService;
    private readonly IFarmerQueryService _farmerQueryService;

    public FarmerService(
        IFarmerQueryService farmerQueryService,
        IFarmerValidationService farmerValidationService,
        IFarmerManagementService farmerManagementService)
    {
        _farmerValidationService = farmerValidationService;
        _farmerManagementService = farmerManagementService;
        _farmerQueryService = farmerQueryService;
    }

    public async Task CreateFarmerAsync(FarmerForCreationDto model, string userId, bool trackChanges) => await
        _farmerManagementService.CreateFarmerAsync(model, userId, trackChanges);

    public async Task<FarmersListViewModel> CreateFarmersListViewModelAsync(IEnumerable<FarmersViewModel> farmers, MetaData metaData, string? searchTerm) => 
        await _farmerQueryService.CreateFarmersListViewModelAsync(farmers, metaData, searchTerm);

    public async Task<(IEnumerable<FarmersViewModel> farmers, MetaData metaData)> GetAllFarmersAsync(FarmerParameters farmerParameters, bool trackChanges) =>
        await _farmerQueryService.GetAllFarmersAsync(farmerParameters, trackChanges);

    public async Task DeleteFarmerAsync(Guid farmerId, bool trackChanges) =>
        await _farmerManagementService.DeleteFarmerAsync(farmerId, trackChanges);

    public async Task<FarmerForUpdatingDto> GetFarmerForEditAsync(Guid farmerId, bool trackChanges) => await
           _farmerQueryService.GetFarmerForEditAsync(farmerId, trackChanges);

    public async Task EditFarmerAsync(FarmerForUpdatingDto model, Guid farmerId, bool trackChanges) => await
        _farmerManagementService.EditFarmerAsync(model,farmerId, trackChanges);

    public async Task<bool> DoesFarmerExistAsync(string egn, string phoneNumber, string userId, bool trackChanges) => await 
        _farmerValidationService.DoesFarmerExistAsync(egn, phoneNumber, userId, trackChanges);


    public async Task<bool> DoesFarmerExistsByuserId(string userId, bool trackChanges) => await
        _farmerValidationService.DoesFarmerExistsByuserId(userId, trackChanges);

    public async Task<FarmerProfileViewModel> GetFarmerProfileAsync(string userId) => await
          _farmerQueryService.GetFarmerProfileAsync(userId);

    public async Task<bool> DoesFarmerHasProductsAsync(string userId, Guid productId, bool trackChanges) => await
        _farmerValidationService.DoesFarmerHasProductsAsync(userId, productId, trackChanges);

    public async Task<FarmerDetailsDto> GetFarmersDetailsByIdAsync(Guid farmerId, bool trackChanges) => await
           _farmerQueryService.GetFarmersDetailsByIdAsync(farmerId, trackChanges);

    public async Task<Guid> GetFarmerByUserIdAsync(Guid userId, bool trackChanges) => await
         _farmerQueryService.GetFarmerByUserIdAsync(userId, trackChanges);
}