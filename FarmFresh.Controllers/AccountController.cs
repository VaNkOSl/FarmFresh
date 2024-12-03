using FarmFresh.Infrastructure.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Controllers;

[Route("api/account")]
public class AccountController : BaseController
{
    private readonly IAccountService _accountService;

    public AccountController( IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("login")]
    public IActionResult Login() =>
        View();

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (await _accountService.Login(model, trackChanges: false))
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid login details!");
        }

        return View(model);
    }

    [HttpGet("register")]
    public IActionResult Register() =>
        View();

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (await _accountService.Register(model, trackChanges: true))
            {
                return RedirectToAction("Index", "Home");
            }
        }

        return View(model);
    }

    [HttpGet("profile/{id?}")]
    public async Task<IActionResult> Profile(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            id = User.GetId()!;
        }

        var userProfile = await _accountService
            .GetUserProfileAsync(id, trackChanges: false);

        if (userProfile is null)
        {
            ViewBag.ErrorMessage = "User profile not found.";
            return View("Error404");
        }

        return View(userProfile);
    }

    [HttpDelete("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromBody] ProfileViewModel model)
    {
        await _accountService.DeleteUserAsync(model.Id, trackChanges: true);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var updateView = await _accountService
            .GetUserForUpdateAsync(id, trackChanges: false);

        return View(updateView);
    }

    [HttpPatch("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] UserForUpdateDto model)
    {
        if (id != model.Id)
        {
            return BadRequest("Route ID and model ID do not match.");
        }

        await _accountService.UpdateUserAsync(model, trackChanges: true);
        return Ok(new { success = true });
    }

    public async Task<IActionResult> Logout()
    {
        await _accountService.Logout();

        return RedirectToAction("Index", "Home");
    }
}
