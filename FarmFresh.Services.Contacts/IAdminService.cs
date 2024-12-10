using FarmFresh.ViewModels.Admin;

namespace FarmFresh.Services.Contacts;

public interface IAdminService
{
    Task<IEnumerable<AdminAllProductDto>> GetUnapprovedProductsAsync(bool trackChanges);

    Task ApproveProductAsync(Guid productId, bool trackChanges);

    Task<AdminRejectViewModel> GetProductForRejecAsync(Guid productId, bool trackChanges);

    Task RejectProductAsync(AdminRejectViewModel model, bool trackChanges);
}
