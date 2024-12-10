using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Areas.Admin.Controllers;

[Route("api/admin/product")]
public class ProductController : AdminBaseController
{
    private readonly IServiceManager _serviceManager;

    public ProductController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;

    }

    [HttpGet("allproduct")]
    public async Task<IActionResult> AllProduct() => 
        View(await 
            _serviceManager
            .AdminService
            .GetUnapprovedProductsAsync(trackChanges: false));

    [HttpPost("approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _serviceManager
            .AdminService.ApproveProductAsync(id, trackChanges: true);

        return RedirectToAction("AllProduct", "Product");
    }

    [HttpGet("reject/{id}")]
    public async Task<IActionResult> Reject(Guid id)
    {
        var model = await _serviceManager
            .AdminService.GetProductForRejecAsync(id, trackChanges: false);

        return View(model);
    }

    [HttpPost("rejectproduct")]
    public async Task<IActionResult> RejectProduct(AdminRejectViewModel model)
    {
        await _serviceManager.AdminService.RejectProductAsync(model, trackChanges: true);
        return RedirectToAction("AllProduct", "Product");
    }

}
