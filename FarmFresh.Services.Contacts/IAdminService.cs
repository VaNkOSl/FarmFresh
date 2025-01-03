using FarmFresh.Commons.RequestFeatures;
using FarmFresh.ViewModels.Admin;
using FarmFresh.ViewModels.User;

namespace FarmFresh.Services.Contacts;

public interface IAdminService
{
    Task<IEnumerable<AdminAllProductDto>> GetUnapprovedProductsAsync(bool trackChanges);

    Task ApproveProductAsync(Guid productId, bool trackChanges);

    Task<AdminRejectProductViewModel> GetProductForRejecAsync(Guid productId, bool trackChanges);

    Task RejectProductAsync(AdminRejectProductViewModel model, bool trackChanges);

    Task<(IEnumerable<AdminAllFarmersDto> farmers, MetaData metaData)> GetUnapprovedFarmersAsync(FarmerParameters farmerParameters, bool trackChanges);

    Task<AdminFarmerListViewModel> CreateAdminFarmersListViewModelAsync(IEnumerable<AdminAllFarmersDto> farmers, MetaData metaData, string? searchTerm);

    Task ApproveFarmerAsync(Guid farmerId, bool trackChanges);

    Task<AdminRejectFarmerDto> GetFarmerForRejectingAsync(Guid farmerId, bool trackChanges);

    Task RejectFarmerAsync(AdminRejectFarmerDto model, bool trackChanges);
}
