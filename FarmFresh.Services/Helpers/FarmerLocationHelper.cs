using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.ViewModels.Farmer;
using LoggerService.Contacts;
using LoggerService.Exceptions.NotFound;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services.Helpers;

public static class FarmerLocationHelper
{
    public static async Task CreateFarmerLocationAsync(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        ILoggerManager loggerManager,
        FarmerCreateLocationDto model,
        Guid farmerId)
    {
        if (farmerId == Guid.Empty)
        {
            loggerManager.LogError($"[{nameof(CreateFarmerLocationAsync)}] farmerId is invalid (null or empty).");
            throw new FarmerIdNotFoundException(farmerId);
        }

        var farmerLocation = mapper.Map<FarmerLocation>(model);
        farmerLocation.FarmerId = farmerId;

        await repositoryManager.FarmerLocationRepository.CreateLocationAsync(farmerLocation);
        await repositoryManager.SaveAsync(farmerLocation);

        loggerManager.LogInfo($"[{nameof(CreateFarmerLocationAsync)}] Successfully created location (ID: {farmerLocation.Id}) for farmer with ID: {farmerId}.");
    }

    public static async Task UpdateFarmerLocationAsync(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        ILoggerManager loggerManager,
        FarmerUpdateLocationDto model,
        Guid farmerId,
        bool trackChanges)
    {
        if (farmerId == Guid.Empty)
        {
            loggerManager.LogError($"[{nameof(UpdateFarmerLocationAsync)}] farmerId is invalid (null or empty).");
            throw new FarmerIdNotFoundException(farmerId);
        }

        var existingLocation = await repositoryManager
            .FarmerLocationRepository
            .FindFarmerLocationsByConditionAsync(fl => fl.FarmerId == farmerId, trackChanges)
            .FirstOrDefaultAsync();

        if (existingLocation is null)
        {
            loggerManager.LogError($"Location for FarmerId {farmerId} was not found.");
            throw new FarmerIdNotFoundException(farmerId);
        }

        mapper.Map(model, existingLocation);

        repositoryManager.FarmerLocationRepository.UpdateLocation(existingLocation);
        await repositoryManager.SaveAsync(existingLocation);

        loggerManager.LogInfo($"[{nameof(UpdateFarmerLocationAsync)}] Location for farmer with ID {farmerId} successfully updated.");
    }

    public static async Task DeleteFarmerLocationAsync(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        ILoggerManager loggerManager,
        Guid farmerId,
        bool trackChanges)
    {
         var locationForDelete = await repositoryManager
            .FarmerLocationRepository
            .FindFarmerLocationsByConditionAsync(fl => fl.FarmerId == farmerId, trackChanges)
            .FirstOrDefaultAsync();

        if(locationForDelete is null)
        {
            loggerManager.LogError($"[{nameof(DeleteFarmerLocationAsync)}] Location for FarmerId {farmerId} was not found.");
            throw new FarmerIdNotFoundException(farmerId);
        }

        repositoryManager.FarmerLocationRepository.DeleteLocation(locationForDelete!);
        await repositoryManager.SaveAsync();
        loggerManager.LogInfo($"Successfully deleted location for FarmerId {farmerId}.");
    }
}
