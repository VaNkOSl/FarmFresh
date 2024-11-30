using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Infrastructure.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Farmer;
using Microsoft.AspNetCore.Mvc;
using static FarmFresh.Commons.MessagesConstants.Farmers;
using static FarmFresh.Commons.NotificationMessagesConstants;

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

        await _serviceManager.FarmerService.CreateFarmerAsync(model, userId, trackChanges: true);
        TempData[SuccessMessage] = SuccessfullyBecomeAFarmer;

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet("allFarmers")]
    public async Task<IActionResult> AllFarmers([FromQuery] FarmerParameters farmerParameters)
    {
        var (farmers, metaData) = await _serviceManager.FarmerService.GetAllFarmersAsync(farmerParameters, trackChanges: false);

        var model = await _serviceManager.FarmerService.CreateFarmersListViewModelAsync(farmers, metaData, farmerParameters.SearchTerm);

        return View(model);
    }
}

