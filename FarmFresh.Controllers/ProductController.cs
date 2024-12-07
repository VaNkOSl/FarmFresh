using FarmFresh.Commons.RequestFeatures;
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

    [HttpGet("allproducts")]
    public async Task<IActionResult> AllProducts([FromQuery] ProductParameters productParameters)
    {
        var (products, metadata) = await _serviceManager.ProductService.GetAllProductsAsync(productParameters, trackChanges: false);

        var model = await _serviceManager.ProductService.CreateProductsViewModelAsync(products, metadata, productParameters.SearchTerm);

        return View(model);
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var model = await _serviceManager.ProductService.GetProductForDeletingAsync(id, trackChanges: false);
        return View(model);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromBody] ProductPreDeleteDto model)
    {
        await _serviceManager.ProductService.DeleteProductAsync(model.Id, trackChanges: true);
        return Ok();
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        return View(await 
            _serviceManager
            .ProductService
            .GetProductForUpdateAsync(id, trackChanges: false));
    }

    [HttpPut("edit/{productId}")]
    public async Task<IActionResult> Edit(Guid productId, UpdateProductDto model)
    {
        if(ModelState.IsValid)
        {
            await _serviceManager.ProductService.UpdateProductAsync(model, productId, trackChanges: true);
        }
        return Ok(new { success = true });
    }

    private async Task LoadModelDataAsync(CreateProductDto model)
    {
        var categories = await _serviceManager
            .ProductService
            .PrepareCreateProductModelAsync(trackChanges: false);

        model.Categories = categories.Categories;
    }
}
