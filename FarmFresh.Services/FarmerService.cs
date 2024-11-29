using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
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

    public async Task CreateFarmerAsync(FarmerCreateForm model, string userId, bool trackChanges)
    {
        if(string.IsNullOrWhiteSpace(userId))
        {
            _loggerManager.LogError($"[{nameof(CreateFarmerAsync)}] Attempted to create farmer, but userId is null or empty!");
            throw new UserIdNotFoundException();
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
                var farmerLocation = new FarmerLocationDto
                {
                  Latitude = model.Latitude.Value,
                  Longitude = model.Longitude.Value,
                  Title = model.Location,
                  FarmerId = farmer.Id,
                };

                await CreateFarmerLocationAsync(farmerLocation, farmer.Id);
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

    public async Task CreateFarmerLocationAsync(FarmerLocationDto model, Guid farmerId)
    {
        var farmerLocation = _mapper.Map<FarmerLocation>(model);
        farmerLocation.FarmerId = farmerId;

        if(farmerId == Guid.Empty)
        {
            _loggerManager.LogError($"[{nameof(CreateFarmerLocationAsync)}] farmerId is invalid (null or empty).");
            throw new UserIdNotFoundException();
        }

        await _repositoryManager.FarmerLocationRepository.CreateLocationAsync(farmerLocation);
        await _repositoryManager.SaveAsync(farmerLocation);
        _loggerManager.LogInfo($"[{nameof(CreateFarmerLocationAsync)}] Successfully created location (ID: {farmerLocation.Id}) for farmer with ID: {farmerId}.");
    }

    public async Task<bool> DoesFarmerExistAsync(string egn, string phoneNumber, string userId, bool trackChanges)
    {
        var farmers = _repositoryManager.FarmerRepository
                                        .FindFarmersByConditionAsync(f => (f.Egn == egn 
                                        || f.PhoneNumber == phoneNumber 
                                        || f.UserId.ToString() == userId), trackChanges);

        return await farmers.AnyAsync();
    }
}
