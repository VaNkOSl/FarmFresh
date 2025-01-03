using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Admin;
using FarmFresh.ViewModels.User;
using LoggerService.Contacts;
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
        var productForApproving = await AdminHelper.GetProductByIdAsync(productId, trackChanges, _repositoryManager, _loggerManager);

        productForApproving.ProductStatus = Status.Approved;
        _repositoryManager.ProductRepository.UpdateProduct(productForApproving);
        await _repositoryManager.SaveAsync();
        _loggerManager.LogInfo($"[{nameof(ApproveProductAsync)}] Successfully approved produc with Id {productId}");
    }

    public async Task<AdminRejectProductViewModel> GetProductForRejecAsync(Guid productId,bool trackChanges)
    {
        var productForReject = await _repositoryManager.ProductRepository
            .FindProductByConditionAsync(p => p.Id == productId, trackChanges)
            .GetProductsWithDetails()
            .FirstOrDefaultAsync();

        ProductHelper.CheckProductNotFound(productForReject, productId, nameof(GetProductForRejecAsync), _loggerManager);

        return _mapper.Map<AdminRejectProductViewModel>(productForReject);
    }

    public async Task<IEnumerable<AdminAllProductDto>> GetUnapprovedProductsAsync(bool trackChanges)
    {
        var products = await _repositoryManager.ProductRepository
            .FindAllProducts(trackChanges)
            .GetProductsWithDetails()
            .Where(p => p.ProductStatus == Status.PendingApproval)
            .ToListAsync();

        return _mapper.Map<List<AdminAllProductDto>>(products);
    }

    public async Task RejectProductAsync(AdminRejectProductViewModel model, bool trackChanges)
    {
        var product = await AdminHelper.GetProductByIdAsync(model.Id, trackChanges, _repositoryManager, _loggerManager);

        product.ProductStatus = Status.Rejected;
        _repositoryManager.ProductRepository.UpdateProduct(product);
        await _repositoryManager.SaveAsync();
       
        await AdminHelper.SendRejectEmailAsync(model, _sendGridApiKey, _loggerManager);
    }

    public async Task<(IEnumerable<AdminAllFarmersDto> farmers, MetaData metaData)> GetUnapprovedFarmersAsync(FarmerParameters farmerParameters, bool trackChanges)
    {
        var farmerWithMetaData = await _repositoryManager.FarmerRepository
            .GetUnapprovedFarmersAsync(farmerParameters, trackChanges);

        var farmerDto = _mapper.Map<IEnumerable<AdminAllFarmersDto>>(farmerWithMetaData);

        _loggerManager.LogInfo("Successfully retrieved and mapped farmers. Returning data.");
        return (farmers: farmerDto, metaData: farmerWithMetaData.MetaData);
    }

    public async Task<AdminFarmerListViewModel> CreateAdminFarmersListViewModelAsync(IEnumerable<AdminAllFarmersDto> farmers, MetaData metaData, string? searchTerm) =>
         _mapper.Map<AdminFarmerListViewModel>((farmers, metaData, searchTerm));

    public async Task ApproveFarmerAsync(Guid farmerId, bool trackChanges)
    {
        var farmer = await AdminHelper.GetFarmerByIdAsync(farmerId, trackChanges, _repositoryManager, _loggerManager);

        farmer.FarmerStatus = Status.Approved;
        _repositoryManager.FarmerRepository.UpdateFarmer(farmer);
        await _repositoryManager.SaveAsync();
        _loggerManager.LogInfo($"[{nameof(ApproveFarmerAsync)}] Successfully approved farmer with Id: {farmerId}.");
    }

    public async Task<AdminRejectFarmerDto> GetFarmerForRejectingAsync(Guid farmerId, bool trackChanges)
    {
        var farmer = await AdminHelper.GetFarmerByIdAsync(farmerId, trackChanges, _repositoryManager, _loggerManager);

        FarmerHelper.ChekFarmerNotFound(farmer, farmerId, nameof(GetFarmerForRejectingAsync), _loggerManager);

        return _mapper.Map<AdminRejectFarmerDto>(farmer);
    }

    public async Task RejectFarmerAsync(AdminRejectFarmerDto model, bool trackChanges)
    {
        var farmer = await AdminHelper.GetFarmerByIdAsync(model.Id, trackChanges, _repositoryManager, _loggerManager);

        if (farmer != null && farmer.FarmerStatus != Status.Approved)
        {
            farmer.FarmerStatus = Status.Rejected;
            _repositoryManager.FarmerRepository.UpdateFarmer(farmer);
            await _repositoryManager.SaveAsync();
        }

        await AdminHelper.SendRejectEmailAsync(model, _sendGridApiKey, _loggerManager);
    }
}
