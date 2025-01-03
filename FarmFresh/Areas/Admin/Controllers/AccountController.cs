using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.User;
using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Areas.Admin.Controllers;

[Route("api/admin/account")]
public class AccountController : AdminBaseController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("allusers")]
    public async Task<IActionResult> AllUsers()
    {
        var users = await _accountService.GetAllUsersAsync(trackChanges: false);

        return View(users);
    }

    [HttpGet("blockuser")]
    public async Task<IActionResult> BlockUser(Guid userId)
    {
        var model = await _accountService.GetUserForBlockAsync(userId, trackChanges: false);
        return View(model);
    }

    [HttpPost("blockuser")]
    public async Task<IActionResult> BlockUser(Guid userId, BlockUserDto model)
    {
        await _accountService.BlockUserAsync(userId, trackChanges: true);
        return RedirectToAction("AllUsers", "Account");
    }
}
