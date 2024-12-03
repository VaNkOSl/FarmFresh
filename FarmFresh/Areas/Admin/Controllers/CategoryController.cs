using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using static FarmFresh.Commons.GeneralApplicationConstants;
using static FarmFresh.Commons.NotificationMessagesConstants;

namespace FarmFresh.Areas.Admin.Controllers;

[Route("api/admin/category")]

public class CategoryController : AdminBaseController
{
    private readonly IServiceManager _serviceManager;

    public CategoryController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("add")]
    public IActionResult AddCategory() =>
        View();

    [HttpGet("All")]
    public async Task<IActionResult> All()
    {
        var result = await _serviceManager
            .CategoryService
            .GetAllCategoriesAsync(trackChanges:false);

        return View(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddCategory(CategoryCreateForm model)
    {
        if(ModelState.IsValid)
        {
            await _serviceManager
                .CategoryService
                .CreateCategoryAsync(model, trackChanges: true);
            TempData[SuccessMessage] = $"Successfully create category with name {model.Name}";
        }

        return RedirectToAction("DashBoard", "Home", new { area = AdminAreaName });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _serviceManager
            .CategoryService
            .DeleteCategory(id, trackChanges: true);
        return Ok();
    }


    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _serviceManager
            .CategoryService
            .GetCategoryForUpdate(id,trackChanges:false);
        return View(result);
    }


    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(CategoryUpdateForm model , Guid id)
    {
        if(ModelState.IsValid)
        {
            await _serviceManager
                .CategoryService
                .UpdateCategory(model, id, trackChanges: true);
        }

        return RedirectToAction("All", "Category", new { area = AdminAreaName });
    }
}
