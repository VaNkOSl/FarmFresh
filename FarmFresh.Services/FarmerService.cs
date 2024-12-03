using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Farmer;
using LoggerService.Contacts;
using LoggerService.Exceptions.BadRequest;
using LoggerService.Exceptions.InternalError;
using LoggerService.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services;

internal sealed class FarmerService : IFarmerService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public FarmerService(IRepositoryManager repositoryManager, ILoggerManager loggerManager,
                         IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task CreateFarmerAsync(FarmerForCreationDto model, string userId, bool trackChanges)
    {
        if(string.IsNullOrWhiteSpace(userId))
        {
            _loggerManager.LogError($"[{nameof(CreateFarmerAsync)}] Attempted to create farmer, but userId is null or empty!");
            throw new UserIdNotFoundException(Guid.Parse(userId));
        }

        if (await DoesFarmerExistAsync(model.Egn, model.PhoneNumber, userId, trackChanges))
        {
            _loggerManager.LogError($"[{nameof(CreateFarmerAsync)}] Farmer with this EGN {model.Egn}, phone number {model.PhoneNumber}, or userId {userId} already exists.");
            throw new FarmerAlreadyExistsException();
        }

        try
        {
            var farmer = _mapper.Map<Farmer>(model);
            farmer.UserId = Guid.Parse(userId);

            await _repositoryManager.FarmerRepository.CreateFarmerAsync(farmer);
            await _repositoryManager.SaveAsync(farmer);

            if (model.Latitude.HasValue && model.Longitude.HasValue)
            {
                var farmerLocation = new FarmerCreateLocationDto
                {
                  Latitude = model.Latitude.Value,
                  Longitude = model.Longitude.Value,
                  Title = model.Location,
                  FarmerId = farmer.Id,
                };

                await FarmerLocationHelper.CreateFarmerLocationAsync(
                    _repositoryManager, 
                    _mapper,
                    _loggerManager,
                    farmerLocation,
                    farmer.Id);
            }

            _loggerManager.LogInfo($"[{nameof(CreateFarmerAsync)}] User with ID {userId} successfully become a farmer!");
        }
        catch (FormatException ex)
        {
            _loggerManager.LogError($"Invalid format for userId: {ex.Message}, Value: {userId}");
            throw new InvalidUserIdFormatException();
        }
        catch(Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new FarmerSomethingWentWrong();
        }
    }

    public async Task<FarmersListViewModel> CreateFarmersListViewModelAsync(IEnumerable<FarmersViewModel> farmers, MetaData metaData, string? searchTerm) => 
         _mapper.Map<FarmersListViewModel>((farmers, metaData, searchTerm));
 
    public async Task<(IEnumerable<FarmersViewModel> farmers, MetaData metaData)> GetAllFarmersAsync(FarmerParameters farmerParameters, bool trackChanges)
    {
        
        var farmerWithMetaData = await _repositoryManager.FarmerRepository.GetFarmersAsync(farmerParameters, trackChanges);

        var farmerDTO = _mapper.Map<IEnumerable<FarmersViewModel>>(farmerWithMetaData);

        _loggerManager.LogInfo("Successfully retrieved and mapped farmers. Returning data.");
        return (farmers: farmerDTO, metaData: farmerWithMetaData.MetaData);
    }

    public async Task DeleteFarmerAsync(Guid farmerId, bool trackChanges)
    {
        var farmerForDeleting = await _repositoryManager
                                      .FarmerRepository
                                      .FindFarmersByConditionAsync(f => f.Id == farmerId, trackChanges)
                                      .FirstOrDefaultAsync();

        if (farmerForDeleting is null)
        {
            _loggerManager.LogError($"[{nameof(DeleteFarmerAsync)}] farmerId is invalid (null or empty)");
            throw new FarmerIdNotFoundException(farmerId);
        }

        try
        {
           await FarmerLocationHelper.DeleteFarmerLocationAsync(
                _repositoryManager,
                _mapper,
                _loggerManager,
                farmerId,
                trackChanges);

            _repositoryManager.FarmerRepository.DeleteFarmer(farmerForDeleting);
            await _repositoryManager.SaveAsync();
            _loggerManager.LogInfo($"Farmer with Id {farmerId} was successfully deleted.");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new DeleteFarmerSomethingWentWrong();
        }
    }

    public async Task<FarmerForUpdatingDto> GetFarmerForEditAsync(Guid farmerId, bool trackChanges)
    {
        var farmerForEdit = await _repositoryManager
                              .FarmerRepository
                              .FindFarmersByConditionAsync(f => f.Id == farmerId, trackChanges)
                              .FirstOrDefaultAsync();

        if (farmerForEdit is null)
        {
            _loggerManager.LogError($"[{nameof(GetFarmerForEditAsync)}] farmer with ID {farmerId} not found!");
            throw new FarmerIdNotFoundException(farmerId);
        }

        return _mapper.Map<FarmerForUpdatingDto>(farmerForEdit);
    }

    public async Task EditFarmerAsync(FarmerForUpdatingDto model, Guid farmerId, bool trackChanges)
    {
        var existingFarmer = await _repositoryManager
                              .FarmerRepository
                              .FindFarmersByConditionAsync(f => f.Id == farmerId, trackChanges)
                              .FirstOrDefaultAsync();

        if (existingFarmer is null)
        {
            _loggerManager.LogError($"[{nameof(EditFarmerAsync)}] Farmer with ID {farmerId} not found.");
            throw new FarmerIdNotFoundException(farmerId);
        }

        try
        {
            _mapper.Map(model, existingFarmer);

            if(model.Latitude.HasValue && model.Longitude.HasValue)
            {
                var updatedLocation = new FarmerUpdateLocationDto
                {
                    Latitude = model.Latitude.Value,
                    Longitude = model.Longitude.Value,
                    Title = model.Location,
                    FarmerId = existingFarmer.Id,
                };

                await FarmerLocationHelper.UpdateFarmerLocationAsync(
                    _repositoryManager,
                    _mapper,
                    _loggerManager,
                    updatedLocation,
                    existingFarmer.Id,
                    trackChanges);
            }

            _repositoryManager.FarmerRepository.UpdateFarmer(existingFarmer);
            await _repositoryManager.SaveAsync();
            _loggerManager.LogInfo($"[{nameof(EditFarmerAsync)}] Farmer with ID {farmerId} successfully updated.");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"[{nameof(EditFarmerAsync)}] Failed to update farmer with ID {farmerId}. Error: {ex.Message}");
            throw new UpdateFarmerSomethingWentWrong();
        }
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

    public async Task<FarmerProfileViewModel> GetFarmerProfileAsync(string userId)
    {
        var farmer = await _repositoryManager
         .FarmerRepository
         .FindFarmersByConditionAsync(f => (f.UserId.ToString() == userId || f.Id.ToString() == userId), trackChanges: false)
         .FirstOrDefaultAsync();

        if (farmer is null)
        {
            _loggerManager.LogError($"[{nameof(GetFarmerProfileAsync)}] Farmer with ID {farmer.Id} not found.");
            throw new FarmerIdNotFoundException(farmer.Id);
        }

        return _mapper.Map<FarmerProfileViewModel>(farmer);
    }
}