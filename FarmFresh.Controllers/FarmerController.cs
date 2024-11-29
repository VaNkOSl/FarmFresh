using FarmFresh.Infrastructure.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Farmer;
using Microsoft.AspNetCore.Mvc;
using static FarmFresh.Commons.NotificationMessagesConstants;
using static FarmFresh.Commons.MessagesConstants.Farmers;

namespace FarmFresh.Controllers;

[Route("api/farmers")]
public class FarmerController : BaseController
{
    private readonly IServiceManager _serviceManager;

    public FarmerController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("become")]
    public IActionResult Become() =>
       View();

    [HttpPost("become")]
    public async Task<IActionResult> Become(FarmerCreateForm model)
    {
        var userId = User.GetId()!;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _serviceManager.FarmerService.CreateFarmerAsync(model, userId, tracktrackChanges: true);
        TempData[SuccessMessage] = SuccessfullyBecomeAFarmer;

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}

