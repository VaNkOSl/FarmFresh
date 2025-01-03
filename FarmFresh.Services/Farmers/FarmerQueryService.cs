using AutoMapper;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Farmer;
using LoggerService.Contacts;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services.Farmers;

public class FarmerQueryService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public FarmerQueryService(IRepositoryManager repositoryManager, ILoggerManager loggerManager,
                         IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task<FarmerProfileViewModel> GetFarmerProfileAsync(string userId)
    {
        var farmer = await _repositoryManager
         .FarmerRepository
         .FindFarmersByConditionAsync(f => (f.UserId.ToString() == userId || f.Id.ToString() == userId), trackChanges: false)
         .Include(u => u.User)
         .FirstOrDefaultAsync();

        FarmerHelper.ChekFarmerNotFound(farmer, Guid.Parse(userId), nameof(GetFarmerProfileAsync), _loggerManager);

        return _mapper.Map<FarmerProfileViewModel>(farmer);
    }

    public async Task<FarmerDetailsDto> GetFarmersDetailsByIdAsync(Guid farmerId, bool trackChanges)
    {
        var farmer = await
            _repositoryManager
            .FarmerRepository
            .FindFarmersByConditionAsync(f => f.Id == farmerId, trackChanges)
            .Include(ow => ow.OwnedProducts)
            .ThenInclude(ph => ph.ProductPhotos)
            .Include(u => u.User)
            .FirstOrDefaultAsync();

        FarmerHelper.ChekFarmerNotFound(farmer, farmerId, nameof(GetFarmersDetailsByIdAsync), _loggerManager);

        return _mapper.Map<FarmerDetailsDto>(farmer);
    }

    public async Task<Guid> GetFarmerByUserIdAsync(Guid userId, bool trackChanges)
    {
        var farmer = await
            _repositoryManager
            .FarmerRepository
            .FindFarmersByConditionAsync(f => f.UserId == userId && f.FarmerStatus == Status.Approved, trackChanges)
            .FirstOrDefaultAsync();

        if (farmer is null)
        {
            return Guid.Empty;
        }

        return farmer.Id;
    }

    public async Task<FarmerForUpdatingDto> GetFarmerForEditAsync(Guid farmerId, bool trackChanges)
    {
        var farmerForEdit = await _repositoryManager
                              .FarmerRepository
                              .FindFarmersByConditionAsync(f => f.Id == farmerId, trackChanges)
                              .FirstOrDefaultAsync();

        FarmerHelper.ChekFarmerNotFound(farmerForEdit, farmerId, nameof(GetFarmerForEditAsync), _loggerManager);

        return _mapper.Map<FarmerForUpdatingDto>(farmerForEdit);
    }
}
