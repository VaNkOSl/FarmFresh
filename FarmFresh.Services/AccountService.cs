using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.User;
using LoggerService.Contacts;
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
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public AccountService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager loggerManager,
                          IPasswordHasher<ApplicationUser> passwordHasher , IHttpContextAccessor httpContextAccessor)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _loggerManager = loggerManager;
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> DoesUserExistAsync(string userName, string email, bool trackChanges)
    {
        var users = _repositoryManager.UserRepository
             .FindUsersByConditionAsync(u => u.UserName == userName 
                                        || u.Email == email, trackChanges);

        Console.WriteLine();


        return await users.AnyAsync();
    }

    public async Task<ProfileViewModel> GetUserProfileAsync(string userId)
    {
        var user = await _repositoryManager.UserRepository.GetUserByIdAsync(Guid.Parse(userId));

        if (user is null)
        {
            _loggerManager.LogWarning($"User with ID {userId} was not found.");
            throw new UserIdNotFound();
        }

        var userProfile = _mapper.Map<ProfileViewModel>(user);

        return userProfile;
    }

    public async Task<bool> Login(LoginViewModel model, bool trackChanges)
    {
        var users = _repositoryManager.UserRepository
                             .FindUsersByConditionAsync(u => u.UserName == model.UserNameOrEmail
                                                        || u.Email == model.UserNameOrEmail, trackChanges);

        var user = await users.FirstOrDefaultAsync();

        if (user is null)
        {
            _loggerManager.LogError("Login attempt failed. User not found.");
            throw new UserNotFounException();
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, model.Password);

        if(verificationResult is PasswordVerificationResult.Failed)
        {
            _loggerManager.LogWarning($"Password verification failed for user {model.UserNameOrEmail}.");
            return false;
        }

        await SignInUserAsync(user);
        _loggerManager.LogInfo($"User {model.UserNameOrEmail} successfully logged in.");

        return true;
    }

    public async Task Logout()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _loggerManager.LogInfo("User successfully logged out.");
    }

    public async Task<bool> Register(RegisterViewModel model, bool trackChanges)
    {
        if (await DoesUserExistAsync(model.UserName, model.Email, trackChanges))
        {
            _loggerManager.LogWarning($"Registration failed. User with username {model.UserName} or email {model.Email} already exists.");
            return false;
        }

        var user = _mapper.Map<ApplicationUser>(model);

        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

        await _repositoryManager.UserRepository.CreateUserAsync(user);
        await _repositoryManager.SaveAsync();

        _loggerManager.LogInfo($"New user registered successfully with username {model.UserName}.");
        return true;
    }

    private async Task SignInUserAsync(ApplicationUser user)
    {
        var claim = new List<Claim>
        {
          new Claim(ClaimTypes.Name, user.UserName!),
          new Claim(ClaimTypes.Email, user.Email!),
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);

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
