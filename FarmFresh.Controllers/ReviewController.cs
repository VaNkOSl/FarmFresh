using FarmFresh.Infrastructure.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Controllers;

[Route("api/reviews")]
public class ReviewController : BaseController
{
    private readonly IServiceManager _serviceManager;

    public ReviewController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductReview(ProductReviewCreateDto model)
    {
        var userId = User.GetId();
        await _serviceManager.ReviewService.CreateProductReviewAsync(model, Guid.Parse(userId), trackChanges: true);
        return RedirectToAction("Details", "Product", new { id = model.ProductId });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _serviceManager.ReviewService.DeleteReview(id, trackChanges: true);

        return Ok();
    }

    [HttpPatch("editproductreview/{id}")]
    public async Task<IActionResult> EditProductReview(Guid id, [FromForm] ProductReviewUpdateDto model)
    {
        await _serviceManager.ReviewService.UpdatereviewAsync(model,id, trackChanges: true);

        return Ok(new { success = true, message = "Review updated successfully." });
    }
}
