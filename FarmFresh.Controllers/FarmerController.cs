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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Become(FarmerForCreationDto model)
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

    [HttpGet("edit/{id}")]

    public async Task<IActionResult> Edit(Guid id)
    {
        var editView =  await _serviceManager.FarmerService.GetFarmerForEditAsync(id, trackChanges: false);
        return View(editView);
    }

    [HttpPatch("edit/{farmerId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid farmerId, FarmerForUpdatingDto model)
    {
        if (ModelState.IsValid)
        {
            await _serviceManager.FarmerService.EditFarmerAsync(model, farmerId, trackChanges: true);
        }
        return Ok(new { success = true });
    }

    [HttpGet("profile/{id}")]
    public async Task<IActionResult> Profile(Guid id)
    {
        var farmerProfile = await _serviceManager.FarmerService.GetFarmerProfileAsync(id.ToString());
        return View(farmerProfile);
    }

    [HttpDelete("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromBody] FarmerProfileViewModel model)
    {
        await _serviceManager.FarmerService.DeleteFarmerAsync(model.Id, trackChanges: true);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var model = await _serviceManager.FarmerService.GetFarmersDetailsByIdAsync(id, trackChanges: false);
        return View(model);
    }
}

