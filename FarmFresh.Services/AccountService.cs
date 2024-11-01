using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.User;
using LoggerService.Contacts;
using LoggerService.Exceptions.BadRequest;
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

    public async Task<ProfileViewModel> GetUserProfileAsync(string userId)
    {
        var user = await _repositoryManager.UserRepository.GetUserByIdAsync(Guid.Parse(userId));

        if (user is null)
        {
            throw new IdParametersBadRequestException();
        }

        var userProfile = _mapper.Map<ProfileViewModel>(user);

        return userProfile;
    }

    public async Task<bool> Login(LoginViewModel model)
    {
        var allUsers = await _repositoryManager.UserRepository.GetAllUsersAsync();

        var user = await allUsers.FirstOrDefaultAsync(u => u.UserName == model.UserNameOrEmail || u.Email == model.UserNameOrEmail);

        if (user == null)
        {
            _loggerManager.LogError("Somenthing went wrock in Login method");
            throw new UserNotFounException();
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, model.Password);

        if(verificationResult is PasswordVerificationResult.Failed)
        {
            return false;
        }

        await SignInUserAsync(user);

        return true;
    }

    public async Task Logout()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<bool> Register(RegisterViewModel model)
    {
        var existingUsers = await _repositoryManager.UserRepository.GetAllUsersAsync();

        if (await existingUsers.AnyAsync(u => u.Email == model.Email || u.UserName == model.UserName))
        {
            return false; 
        }

        var user = _mapper.Map<ApplicationUser>(model);

        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

        await _repositoryManager.UserRepository.CreateUserAsync(user);
        await _repositoryManager.SaveAsync();

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
