using FarmFresh.Infrastructure.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Controllers;

[Route("api/products")]
public class ProductController : BaseController
{
    private readonly IServiceManager _serviceManager;

    public ProductController(IServiceManager serviceManager)
    {
       _serviceManager = serviceManager;
    }

    [HttpGet("add")]
    public async Task<IActionResult> Add() => 
        View(await _serviceManager
            .ProductService
            .PrepareCreateProductModelAsync(trackChanges: false));

    [HttpPost("add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(CreateProductDto model)
    {
        var userId = User.GetId()!;

        if(!ModelState.IsValid)
        {
            await LoadModelDataAsync(model);
            return View(model);
        }

        await _serviceManager.ProductService.CreateProductAsync(model, userId, trackChanges: true);

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
    
    private async Task LoadModelDataAsync(CreateProductDto model)
    {
        var categories = await _serviceManager
            .ProductService
            .PrepareCreateProductModelAsync(trackChanges: false);

        model.Categories = categories.Categories;
    }
}
