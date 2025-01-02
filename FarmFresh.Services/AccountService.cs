using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.Services.Helpers;
using FarmFresh.ViewModels.User;
using LoggerService.Contacts;
using LoggerService.Exceptions.InternalError.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Web;

namespace FarmFresh.Services;

public sealed class AccountService : IAccountService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly string _sendGridApiKey;
    public AccountService(IRepositoryManager repositoryManager,
                          IMapper mapper,
                          ILoggerManager loggerManager,
                          IPasswordHasher<ApplicationUser> passwordHasher, 
                          IHttpContextAccessor httpContextAccessor,
                          UserManager<ApplicationUser> userManager,
                          IConfiguration configuration)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _loggerManager = loggerManager;
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _sendGridApiKey = configuration["SendGrid:ApiKey"];
    }

    public async Task<bool> Login(LoginViewModel model, bool trackChanges)
    {
        var user = await _repositoryManager
            .UserRepository
            .FindUsersByConditionAsync(u => u.UserName == model.UserNameOrEmail || 
            u.Email == model.UserNameOrEmail, 
            trackChanges).FirstOrDefaultAsync();

        AccountHelper.ChekIfUserIsNull(user, user.Id, nameof(Login), _loggerManager);

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

        AccountHelper.ChekIfUserIsNull(user, Guid.Parse(userId), nameof(GetUserProfileAsync), _loggerManager);

        return _mapper.Map<ProfileViewModel>(user);
    }

    public async Task DeleteUserAsync(Guid userId, bool trackChanges)
    {
        var userForDeleting = await
            _repositoryManager
            .UserRepository
            .FindUsersByConditionAsync(u => u.Id == userId, trackChanges)
            .FirstOrDefaultAsync();

        await DeleteFarmerAsync(userId, trackChanges);
        AccountHelper.ChekIfUserIsNull(userForDeleting, userId, nameof(DeleteUserAsync), _loggerManager);

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

    private async Task DeleteFarmerAsync(Guid userId, bool trackChanges)
    {
        var farmerForDeleting = await 
            _repositoryManager
            .FarmerRepository
            .FindFarmersByConditionAsync(f => f.UserId == userId, trackChanges)
            .SingleOrDefaultAsync();

        AccountHelper.ChekIfUserIsNull(farmerForDeleting, userId, nameof(DeleteFarmerAsync), _loggerManager);

        _repositoryManager.FarmerRepository.DeleteFarmer(farmerForDeleting);
        await _repositoryManager.SaveAsync();
    }

    public async Task<UserForUpdateDto> GetUserForUpdateAsync(Guid userId, bool trackChanges)
    {
        var userForUpdate = await
            _repositoryManager
            .UserRepository
            .FindUsersByConditionAsync(u => u.Id == userId, trackChanges)
            .FirstOrDefaultAsync();

        AccountHelper.ChekIfUserIsNull(userForUpdate, userId, nameof(GetUserForUpdateAsync), _loggerManager);

        return _mapper.Map<UserForUpdateDto>(userForUpdate);
    }

    public async Task UpdateUserAsync(UserForUpdateDto model, bool trackChanges)
    {
        var currentUserForUpdate = await
          _repositoryManager
          .UserRepository
          .FindUsersByConditionAsync(u => u.Id == model.Id, trackChanges)
          .FirstOrDefaultAsync();

        AccountHelper.ChekIfUserIsNull(currentUserForUpdate, model.Id, nameof(UpdateUserAsync), _loggerManager);

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

    public async Task ForgotPasswordAsync(string email, bool trackChanges)
    {
        var user = await
            _repositoryManager
            .UserRepository
            .FindUsersByConditionAsync(u => u.Email == email, trackChanges)
            .FirstOrDefaultAsync();

        AccountHelper.ChekIfUserIsNull(user, user.Id, nameof(ForgotPasswordAsync), _loggerManager);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var request = _httpContextAccessor.HttpContext.Request;
        var resetLink = $"{request.Scheme}://{request.Host}/api/account/resetpassword?token={HttpUtility.UrlEncode(token)}&email={email}";

        _loggerManager.LogInfo($"Reset Password Link: {resetLink}");

        var model = new PasswordResetEmailModel
        {
          EmailFrom = "contactfarmfresh2024@abv.bg",
          EmailTo = email,
          EmailSubject = "Password Reset",
          EmailBody = $"Click the link to reset your password: <a href='{resetLink}'>Reset Password</a>"
        };

        await AdminHelper.SendRejectEmailAsync(model, _sendGridApiKey, _loggerManager);
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword, bool trackChanges)
    {
        var user = await _userManager.FindByEmailAsync(email);

        AccountHelper.ChekIfUserIsNull(user, user.Id, nameof(ForgotPasswordAsync), _loggerManager);

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (result.Succeeded)
        {
            return true;
        }

        foreach (var error in result.Errors)
        {
            _loggerManager.LogError($"Password reset failed for {email}: {error.Description}");
        }

        return false;
    }

    public async Task<IEnumerable<AllUserDto>> GetAllUsersAsync(bool trackChanges)
    {
        var users = await _repositoryManager.UserRepository
              .GetAllUsers(trackChanges)
              .Where(u => u.IsBlocked == false)
              .ToListAsync();

        var farmerUserIds = await _repositoryManager.FarmerRepository
            .FindAllFarmers(trackChanges)
            .Select(f => f.UserId)
            .ToListAsync();

        var userDtos = new List<AllUserDto>();
        foreach (var user in users)
        {
            var dto = _mapper.Map<AllUserDto>(user);

            if (farmerUserIds.Contains(user.Id))
            {
                dto.IsSeller = true;

                var farmer = await _repositoryManager.FarmerRepository
                    .FindFarmersByConditionAsync(f => f.UserId == user.Id, trackChanges)
                    .FirstOrDefaultAsync();

                dto.PhoneNumber = farmer?.PhoneNumber ?? string.Empty;
            }
            else
            {
                dto.IsSeller = false;
                dto.PhoneNumber = string.Empty;
            }

            userDtos.Add(dto);
        }

        return userDtos;
    }
    public async Task<bool> IsUserAdmin(string userId, bool trackChanges) =>
        await _repositoryManager.UserRepository
        .FindUsersByConditionAsync(u => u.Id.ToString() == userId && u.IsAdmin == true, trackChanges)
        .AnyAsync();

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
