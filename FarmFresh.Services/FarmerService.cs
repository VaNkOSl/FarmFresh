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

    public async Task CreateFarmerAsync(FarmerCreateUpdateForm model, string userId, bool trackChanges)
    {
        if(string.IsNullOrWhiteSpace(userId))
        {
            _loggerManager.LogError("Attempted to create farmer, but userId is null or empty!");
            throw new UserIdNotFound();
        }

        if (await DoesFarmerExistAsync(model.Egn, model.PhoneNumber, userId, trackChanges))
        {
            _loggerManager.LogError($"Farmer with this EGN {model.Egn}, phone number {model.PhoneNumber}, or userId {userId} already exists.");
            throw new FarmerAlreadyExistsException();
        }

        try
        {
            var farmer = _mapper.Map<Farmer>(model);
            farmer.UserId = Guid.Parse(userId);

            await _repositoryManager.FarmerRepository.CreateFarmerAsync(farmer);
            await _repositoryManager.SaveAsync(farmer);
            _loggerManager.LogInfo($"User with ID {userId} successfully become a farmer!");
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

    public async Task<bool> DoesFarmerExistAsync(string egn, string phoneNumber, string userId, bool trackChanges)
    {
        var farmers = _repositoryManager.FarmerRepository
                                        .FindFarmersByConditionAsync(f => (f.Egn == egn 
                                        || f.PhoneNumber == phoneNumber 
                                        || f.UserId.ToString() == userId), trackChanges);

        return await farmers.AnyAsync();
    }
}
