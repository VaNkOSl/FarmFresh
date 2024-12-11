using FarmFresh.Commons.RequestFeatures;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Areas.Admin.Controllers;

[Route("api/admin/farmers")]
public class FarmerController : AdminBaseController
{
    private readonly IServiceManager _serviceManager;

    public FarmerController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("farmersforreview")]
    public async Task<IActionResult> FarmersForReview([FromQuery] FarmerParameters farmerParameters)
    {
        var (farmer, metadata) = await _serviceManager.AdminService.GetUnapprovedFarmersAsync(farmerParameters, trackChanges: false);

        var model = await _serviceManager.AdminService.CreateAdminFarmersListViewModelAsync(farmer, metadata, farmerParameters.SearchTerm);

        return View(model);
    }

    [HttpPost("approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _serviceManager.AdminService.ApproveFarmerAsync(id, trackChanges: true);

        return RedirectToAction("FarmersForReview", "Farmer");
    }

    [HttpGet("reject/{id}")]
    public async Task<IActionResult> Reject(Guid id)
    {
        var model = await 
            _serviceManager.AdminService.GetFarmerForRejectingAsync(id, trackChanges: false);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RejectFarmer(AdminRejectFarmerDto model)
    {
        await _serviceManager.AdminService.RejectFarmerAsync(model, trackChanges: true);
        return RedirectToAction("FarmersForReview", "Farmer");
    }
}
