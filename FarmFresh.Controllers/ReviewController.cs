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

    [HttpGet("allreviews")]
    public async Task<IActionResult> AllReviews()
    {
        var user = User.GetId();
        var model = await _serviceManager.ReviewService.GetAllReviewsAsync(Guid.Parse(user), trackChanges: false);

        return View(model);
    }

    [HttpGet("leavereview/{productId}")]
    public async Task<IActionResult> LeaveReview(Guid productId)
    {
        var model = new ProductReviewCreateDto
        {
            ProductId = productId
        };

        return View(model);
    }

    [HttpPost("leavereview/{productId}")]
    public async Task<IActionResult> LeaveReview(ProductReviewCreateDto model, Guid productId)
    {
        var userId = User.GetId();
        await _serviceManager.ReviewService.CreateProductReviewAsync(model, Guid.Parse(userId), trackChanges: true);
        return RedirectToAction("AllReviews", "Review");
    }

    [HttpGet("getreviews")]
    public async Task<IActionResult> GetReviews()
    {
        var userId = User.GetId();

        var model = await _serviceManager.ReviewService.GetReviewedProductsAsync(Guid.Parse(userId), trackChanges: false);

        return View(model);
    }
}
