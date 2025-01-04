using AutoMapper;
using FarmFresh.Repositories.Contacts;
using LoggerService.Contacts;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services.Farmers;

public class FarmerValidationService
{
    private readonly IRepositoryManager _repositoryManager;

    public FarmerValidationService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<bool> DoesFarmerExistAsync(string egn, string phoneNumber, string userId, bool trackChanges) =>
        await _repositoryManager
        .FarmerRepository
        .FindFarmersByConditionAsync(f => (f.Egn == egn ||
        f.PhoneNumber == phoneNumber ||
        f.UserId.ToString() == userId), trackChanges)
        .AnyAsync();


    public async Task<bool> DoesFarmerExistsByuserId(string userId, bool trackChanges) =>
        await _repositoryManager
        .FarmerRepository
        .FindFarmersByConditionAsync(f => f.UserId.ToString() == userId, trackChanges)
        .AnyAsync();

    public async Task<bool> DoesFarmerHasProductsAsync(string userId, Guid productId, bool trackChanges)
    {
        var farmer = await
            _repositoryManager
            .FarmerRepository
            .FindFarmersByConditionAsync(f => f.UserId.ToString() == userId, trackChanges)
            .Include(f => f.OwnedProducts)
            .FirstOrDefaultAsync();

        if (farmer is null)
        {
            return false;
        }

        return farmer.OwnedProducts.Any(op => op.Id == productId);
    }
}
