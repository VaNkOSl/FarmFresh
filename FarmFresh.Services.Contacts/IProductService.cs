using FarmFresh.ViewModels.Product;

namespace FarmFresh.Services.Contacts;

public interface IProductService
{
    Task CreateProductAsync(CreateProductDto model, string userId, bool trackChanges);

    Task<CreateProductDto> PrepareCreateProductModelAsync(bool  trackChanges);
}
