using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Admin;
using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FarmFresh.Services;

internal sealed class AdminService : IAdminService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    private readonly string _sendGridApiKey;

    public AdminService(IRepositoryManager repositoryManager,
                        ILoggerManager loggerManager,
                        IMapper mapper, IConfiguration configuration)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
        _sendGridApiKey = configuration["SendGrid:ApiKey"];
    }

    public async Task ApproveProductAsync(Guid productId, bool trackChanges)
    {
        var productForApproving = await
            _repositoryManager
            .ProductRepository
            .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
            .FirstOrDefaultAsync();

        CheckProductNotFound(productForApproving, productId, nameof(ApproveProductAsync));

        productForApproving.ProductStatus = Status.Approved;
        _repositoryManager.ProductRepository.UpdateProduct(productForApproving);
        await _repositoryManager.SaveAsync();
        _loggerManager.LogInfo($"[{nameof(ApproveProductAsync)}] Successfully approved produc with Id {productId}");
    }

    public async Task<AdminRejectProductViewModel> GetProductForRejecAsync(Guid productId,bool trackChanges)
    {
        var productForReject = await
            _repositoryManager
            .ProductRepository
            .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
            .Include(c => c.Category)
            .Include(f => f.Farmer)
            .ThenInclude(u => u.User)
            .Include(ph => ph.ProductPhotos)
            .FirstOrDefaultAsync();

        CheckProductNotFound(productForReject, productId, nameof(ApproveProductAsync));

        return _mapper.Map<AdminRejectProductViewModel>(productForReject);
    }

    public async Task<IEnumerable<AdminAllProductDto>> GetUnapprovedProductsAsync(bool trackChanges)
    {
        var products = await
            _repositoryManager
            .ProductRepository
            .FindAllProducts(trackChanges)
            .Where(p => p.ProductStatus == Status.PendingApproval)
            .Include(f => f.Farmer)
            .Include(c => c.Category)
            .Include(ph => ph.ProductPhotos)
            .ToListAsync();

        return _mapper.Map<List<AdminAllProductDto>>(products);
    }

    public async Task RejectProductAsync(AdminRejectProductViewModel model, bool trackChanges)
    {
        var product = await
            _repositoryManager
            .ProductRepository
            .FindProductByConditionAsync(p => p.Id == model.Id, trackChanges)
            .FirstOrDefaultAsync();


        if (product != null && product.ProductStatus != Status.Approved)
        {
            product.ProductStatus = Status.Rejected;
            _repositoryManager.ProductRepository.UpdateProduct(product);
            await _repositoryManager.SaveAsync();
        }

        await AdminHelper.SendRejectEmailAsync(model, _sendGridApiKey, _loggerManager);
    }

    public async Task<(IEnumerable<AdminAllFarmersDto> farmers, MetaData metaData)> GetUnapprovedFarmersAsync(FarmerParameters farmerParameters, bool trackChanges)
    {
        var farmerWithMetaData = await
            _repositoryManager
            .FarmerRepository
            .GetUnapprovedFarmersAsync(farmerParameters, trackChanges);

        var farmerDto = _mapper.Map<IEnumerable<AdminAllFarmersDto>>(farmerWithMetaData);

        _loggerManager.LogInfo("Successfully retrieved and mapped farmers. Returning data.");
        return (farmers: farmerDto, metaData: farmerWithMetaData.MetaData);
    }

    public async Task<AdminFarmerListViewModel> CreateAdminFarmersListViewModelAsync(IEnumerable<AdminAllFarmersDto> farmers, MetaData metaData, string? searchTerm) =>
         _mapper.Map<AdminFarmerListViewModel>((farmers, metaData, searchTerm));

    public async Task ApproveFarmerAsync(Guid farmerId, bool trackChanges)
    {
        var farmer = await
            _repositoryManager
            .FarmerRepository
            .FindFarmersByConditionAsync(f => f.Id == farmerId, trackChanges)
            .FirstOrDefaultAsync();

        CheckFarmerNotFound(farmer, farmerId, nameof(ApproveFarmerAsync));

        farmer.FarmerStatus = Status.Approved;
        _repositoryManager.FarmerRepository.UpdateFarmer(farmer);
        await _repositoryManager.SaveAsync();
        _loggerManager.LogInfo($"[{nameof(ApproveFarmerAsync)}] Successfully approved farmer with Id: {farmerId}.");
    }

    public async Task<AdminRejectFarmerDto> GetFarmerForRejectingAsync(Guid farmerId, bool trackChanges)
    {
        var farmer = await
            _repositoryManager
            .FarmerRepository
            .FindFarmersByConditionAsync(f => f.Id == farmerId, trackChanges)
            .Include(u => u.User)
            .FirstOrDefaultAsync();

        CheckFarmerNotFound(farmer, farmerId, nameof(ApproveFarmerAsync));

        return _mapper.Map<AdminRejectFarmerDto>(farmer);
    }

    public async Task RejectFarmerAsync(AdminRejectFarmerDto model, bool trackChanges)
    {
        var farmer = await
            _repositoryManager
            .FarmerRepository
            .FindFarmersByConditionAsync(f => f.Id == model.Id, trackChanges)
            .Include(u => u.User)
            .FirstOrDefaultAsync();

        CheckFarmerNotFound(farmer, model.Id, nameof(ApproveFarmerAsync));

        if (farmer != null && farmer.FarmerStatus != Status.Approved)
        {
            farmer.FarmerStatus = Status.Rejected;
            _repositoryManager.FarmerRepository.UpdateFarmer(farmer);
            await _repositoryManager.SaveAsync();
        }

        await AdminHelper.SendRejectEmailAsync(model, _sendGridApiKey, _loggerManager);
    }

    private void CheckProductNotFound(object product, Guid productId, string methodName)
    {
        if (product is null)
        {
            _loggerManager.LogError($"[{methodName}] Product with Id {productId} was not found at Date: {DateTime.UtcNow}");
            throw new ProductIdNotFoundException(productId);
        }
    }

    private void CheckFarmerNotFound(object farmer, Guid farmerId, string methodName)
    {
        if (farmer is null)
        {
            _loggerManager.LogError($"[{nameof(methodName)}] farmerId with Id {farmerId} was not found!");
            throw new FarmerIdNotFoundException(farmerId);
        }
    }
}
