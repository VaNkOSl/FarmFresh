namespace FarmFresh.Services.Contacts.FarmersInterfaces;

public interface IFarmerValidationService
{
    Task<bool> DoesFarmerExistAsync(string egn, string phoneNumber, string userId, bool trackChanges);

    Task<bool> DoesFarmerExistsByuserId(string userId, bool trackChanges);

    Task<bool> DoesFarmerHasProductsAsync(string userId, Guid productId, bool trackChanges);
}

