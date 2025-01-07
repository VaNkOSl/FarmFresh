using FarmFresh.ViewModels.Product;

namespace FarmFresh.Services.Contacts.ProductsInterfaces;

public interface IProductManagmentService
{
    Task CreateProductAsync(CreateProductDto model, string userId, bool trackChanges);

    Task DeleteProductAsync(Guid productId, bool trackChanges);

    Task UpdateProductAsync(UpdateProductDto model, Guid productId, bool trackChanges);
}
