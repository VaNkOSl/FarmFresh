using AutoMapper;
using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.Farmer;
using LoggerService.Contacts;
using LoggerService.Exceptions.BadRequest;
using LoggerService.Exceptions.InternalError.Farmers;
using LoggerService.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services.Farmers;

public class FarmerManagementService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;
    private readonly FarmerValidationService _farmerValidationService;

    public FarmerManagementService(IRepositoryManager repositoryManager, ILoggerManager loggerManager,
                         IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
        _farmerValidationService = new FarmerValidationService(_repositoryManager, _loggerManager, _mapper);
    }

    public async Task CreateFarmerAsync(FarmerForCreationDto model, string userId, bool trackChanges)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            _loggerManager.LogError($"[{nameof(CreateFarmerAsync)}] Attempted to create farmer, but userId is null or empty!");
            throw new UserIdNotFoundException(Guid.Parse(userId));
        }

        if (await _farmerValidationService.DoesFarmerExistAsync(model.Egn, model.PhoneNumber, userId, trackChanges))
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
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new FarmerSomethingWentWrong();
        }
    }

    public async Task DeleteFarmerAsync(Guid farmerId, bool trackChanges)
    {
        var farmerForDeleting = await _repositoryManager
                                      .FarmerRepository
                                      .FindFarmersByConditionAsync(f => f.Id == farmerId, trackChanges)
                                      .FirstOrDefaultAsync();

        FarmerHelper.ChekFarmerNotFound(farmerForDeleting, farmerId, nameof(DeleteFarmerAsync), _loggerManager);

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

    public async Task EditFarmerAsync(FarmerForUpdatingDto model, Guid farmerId, bool trackChanges)
    {
        var existingFarmer = await _repositoryManager
                              .FarmerRepository
                              .FindFarmersByConditionAsync(f => f.Id == farmerId, trackChanges)
                              .FirstOrDefaultAsync();

        FarmerHelper.ChekFarmerNotFound(existingFarmer, farmerId, nameof(EditFarmerAsync), _loggerManager);

        try
        {
            _mapper.Map(model, existingFarmer);

            if (model.Latitude.HasValue && model.Longitude.HasValue)
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
                    farmerId,
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

    public async Task<(IEnumerable<FarmersViewModel> farmers, MetaData metaData)> GetAllFarmersAsync(FarmerParameters farmerParameters, bool trackChanges)
    {

        var farmerWithMetaData = await _repositoryManager.FarmerRepository.GetFarmersAsync(farmerParameters, trackChanges);

        var farmerDTO = _mapper.Map<IEnumerable<FarmersViewModel>>(farmerWithMetaData);

        _loggerManager.LogInfo("Successfully retrieved and mapped farmers. Returning data.");
        return (farmers: farmerDTO, metaData: farmerWithMetaData.MetaData);
    }
}
