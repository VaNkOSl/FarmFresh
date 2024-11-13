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

    public async Task CreateFarmerAsync(FarmerCreateUpdateForm model, string userId)
    {
        if(string.IsNullOrWhiteSpace(userId))
        {
            _loggerManager.LogError("Attempted to create farmer, but userId is null or empty!");
            throw new UserIdNotFound();
        }

        if(await FarmerEgnAlreadyExistsAsync(model.Egn))
        {
            _loggerManager.LogError($"Farmer with this EGN {model.Egn} already exists.");
            throw new EgnAlreadyExistsException();
        }

        if(await FarmerPhoneNumberAlreadyExists(model.PhoneNumber))
        {
            _loggerManager.LogError($"Farmer with this phone number {model.PhoneNumber} already exists.");
            throw new PhoneNumberAlreadyExistsException();
        }

        if(await FarmerWithProvidedUserIdAlreadyExistsAsync(userId))
        {
            _loggerManager.LogError($"Farmer with this userId {userId} already exists.");
            throw new UserIdAlreadyExistsException();
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
            _loggerManager.LogError($"Invalid format for userId: {ex.Message}");
            throw new InvalidUserIdFormatException();
        }
        catch(Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new FarmerSomethingWentWrong();
        }
    }

    public async Task<bool> FarmerEgnAlreadyExistsAsync(string egn)
    {
        var farmers = await _repositoryManager.FarmerRepository.GetAllFarmerReadOnlyAsync();

        return await farmers.AnyAsync(f => f.Egn == egn);
    }

    public async Task<bool> FarmerPhoneNumberAlreadyExists(string phoneNumber)
    {
        var farmers = await _repositoryManager.FarmerRepository.GetAllFarmerReadOnlyAsync();

       return await farmers.AnyAsync(f => f.PhoneNumber == phoneNumber);
    }

    public async Task<bool> FarmerWithProvidedUserIdAlreadyExistsAsync(string userId)
    {
        var farmers = await _repositoryManager.FarmerRepository.GetAllFarmerReadOnlyAsync();

        return await farmers.AnyAsync(f => f.UserId.ToString() == userId);
    }
}
