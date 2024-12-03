using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.User;
using LoggerService.Contacts;
using LoggerService.Exceptions.InternalError.Users;
using LoggerService.Exceptions.NotFound;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FarmFresh.Services;

public sealed class AccountService : IAccountService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(IRepositoryManager repositoryManager,
                          IMapper mapper,
                          ILoggerManager loggerManager,
                          IPasswordHasher<ApplicationUser> passwordHasher, 
                          IHttpContextAccessor httpContextAccessor,
                          UserManager<ApplicationUser> userManager)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _loggerManager = loggerManager;
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<bool> Login(LoginViewModel model, bool trackChanges)
    {
        var user = await _repositoryManager
            .UserRepository
            .FindUsersByConditionAsync(u => u.UserName == model.UserNameOrEmail || 
            u.Email == model.UserNameOrEmail, 
            trackChanges).FirstOrDefaultAsync();

        if (user is null)
        {
            _loggerManager.LogError($"[{nameof(Login)}] Login attempt failed. User not found.");
            throw new UserNotFounException();
        }

        try
        {
            var verificationResult = _passwordHasher
                .VerifyHashedPassword(user, user.PasswordHash!, model.Password);

            if (verificationResult is PasswordVerificationResult.Failed)
            {
                _loggerManager.LogWarning($"Password verification failed for user {model.UserNameOrEmail}.");
                return false;
            }

            await SignInUserAsync(user);
            _loggerManager.LogInfo($"User {model.UserNameOrEmail} successfully logged in at Date: {DateTime.UtcNow}.");

            return true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"[{nameof(Login)}] Error occurred during login attempt for {model.UserNameOrEmail}: {ex.Message}");
            throw new LoginFailedException();
        }
    }

    public async Task<bool> Register(RegisterViewModel model, bool trackChanges)
    {
        if (await DoesUserExistAsync(model.UserName, model.Email, trackChanges))
        {
            _loggerManager.LogWarning($"Registration failed. User with username {model.UserName} or email {model.Email} already exists.");
            return false;
        }

        try
        {
            var user = _mapper.Map<ApplicationUser>(model);

            user.PasswordHash = _passwordHasher
                .HashPassword(user, model.Password);

            await _repositoryManager.UserRepository.CreateUserAsync(user);
            await _repositoryManager.SaveAsync();

            _loggerManager.LogInfo($"New user registered successfully with username {model.UserName} at Date: {DateTime.UtcNow}.");
            return true;
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"[{nameof(Register)}] Error occurred during registration for username {model.UserName}: {ex.Message}");
            throw new RegistrationFailedException();
        }
    }

    public async Task<bool> DoesUserExistAsync(string userName, string email, bool trackChanges) =>
        await _repositoryManager
        .UserRepository
        .FindUsersByConditionAsync(u => u.UserName == userName ||
        u.Email == email, trackChanges)
        .AnyAsync();

    public async Task<ProfileViewModel> GetUserProfileAsync(string userId, bool trackChanges)
    {
        var user = await _repositoryManager
            .UserRepository
            .FindUsersByConditionAsync(u => u.Id.ToString() == userId,
            trackChanges)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            _loggerManager.LogWarning($"User with ID {userId} was not found.");
            throw new UserIdNotFoundException(Guid.Parse(userId));
        }

        return _mapper.Map<ProfileViewModel>(user);
    }

    public async Task DeleteUserAsync(Guid userId, bool trackChanges)
    {
        var userForDeleting = await
            _repositoryManager
            .UserRepository
            .FindUsersByConditionAsync(u => u.Id == userId, trackChanges)
            .FirstOrDefaultAsync();

        var farmerForDeleting = await
            _repositoryManager
            .FarmerRepository
            .FindFarmersByConditionAsync(f => f.UserId == userId, trackChanges)
            .FirstOrDefaultAsync();

        if (farmerForDeleting != null)
        {
            _repositoryManager.FarmerRepository.DeleteFarmer(farmerForDeleting);
            _loggerManager.LogInfo($"[{nameof(DeleteUserAsync)}] Farmer linked to user with ID {userId} successfully deleted.");
        }

        if (userForDeleting is null)
        {
            _loggerManager.LogError($"[{nameof(DeleteUserAsync)}] User with Id {userId} was not found!");
            throw new UserIdNotFoundException(userId);
        }

        try
        {
            _repositoryManager.UserRepository.DeleteUser(userForDeleting);
            await _repositoryManager.SaveAsync();
            _loggerManager.LogInfo($"User with ID {userId} successfully deleted at Date: {DateTime.UtcNow}.");
            await Logout();
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"[{nameof(DeleteUserAsync)}] Error while deleting user with ID {userId}: {ex.Message}");
            throw new DeleteUserSomethingWentWrong();
        }

 
    }

    public async Task<UserForUpdateDto> GetUserForUpdateAsync(Guid userId, bool trackChanges)
    {
        var userForUpdate = await
            _repositoryManager
            .UserRepository
            .FindUsersByConditionAsync(u => u.Id == userId, trackChanges)
            .FirstOrDefaultAsync();

        if (userForUpdate is null)
        {
            _loggerManager.LogError($"[{nameof(GetUserForUpdateAsync)}] User with Id {userId} was not found!");
            throw new UserIdNotFoundException(userId);
        }

        return _mapper.Map<UserForUpdateDto>(userForUpdate);
    }

    public async Task UpdateUserAsync(UserForUpdateDto model, bool trackChanges)
    {
        var currentUserForUpdate = await
          _repositoryManager
          .UserRepository
          .FindUsersByConditionAsync(u => u.Id == model.Id, trackChanges)
          .FirstOrDefaultAsync();

        if (currentUserForUpdate is null)
        {
            _loggerManager.LogError($"[{nameof(DeleteUserAsync)}] User with Id {model.Id} was not found!");
            throw new UserIdNotFoundException(model.Id);
        }

        try
        {
            _mapper.Map(model, currentUserForUpdate);
            _repositoryManager.UserRepository.UpdateUser(currentUserForUpdate);
            await _repositoryManager.SaveAsync();
            _loggerManager.LogInfo($"[{nameof(UpdateUserAsync)}] Successfully updated user with Id {currentUserForUpdate.Id} at Date: {DateTime.UtcNow}");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"[{nameof(UpdateUserAsync)}] Error while updating user with ID {model.Id}: {ex.Message}");
            throw new UpdateUserException();
        }
    }

    public async Task Logout()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _loggerManager.LogInfo("User successfully logged out.");
    }

    private async Task SignInUserAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
        };

        await _httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );
    }
}
