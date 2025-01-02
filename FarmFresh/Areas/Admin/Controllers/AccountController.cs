using FarmFresh.Services.Contacts;
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
}
