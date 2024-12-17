using FarmFresh.Data.Models;
using FarmFresh.Infrastructure.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmFresh.Controllers;

[Authorize]
[Route("api/order")]
public class OrderController : Controller
{
    private readonly IServiceManager _serviceManager;

    public OrderController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("index")]
    public async Task<IActionResult> Index()
    {
        var userId = Guid.Parse(User.GetId());
        var orders = await _serviceManager.OrderService.GetOrdersForUserAsync(userId, trackChanges: false);
        return View(orders);
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details(Guid Id)
    {
        var orderDetails = await _serviceManager.OrderService.GetOrderDetailsAsync(Id, trackChanges: false);

        if (orderDetails == null)
        {
            return NotFound();
        }

        return View(new List<OrderDetailsViewModel> { orderDetails });
    }

    [HttpPost("checkout")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CreateOrderDto model, Guid productId)
    {

        var userId = Guid.Parse(User.GetId());

        try
        {
            var orderId = await _serviceManager.OrderService.CheckoutAsync(model, userId, trackChanges: true);
            return RedirectToAction("OrderConfirmation", new { id = orderId });

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
        }


        return View(model);
    }

    [HttpGet("orderconfirmation")]
    public async Task<IActionResult> OrderConfirmation(Guid id)
    {
        var orderViewModel = await _serviceManager.OrderService.GetOrderConfirmationViewModelAsync(id, trackChanges: false);

        if (orderViewModel == null)
        {
            return NotFound();
        }

        return View(orderViewModel);
    }

    [HttpPost("orderconfirmation")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OrderConfirmation(Order order)
    {
        try
        {
            await _serviceManager.OrderService.CompleteOrderAsync(order.Id, trackChanges: true);
            return RedirectToAction("Index", "Home");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while completing your order. Please try again.");
            return View("Error");
        }
    }

    [HttpGet("getorderbyseller")]
    public async Task<IActionResult> GetOrderBySeller()
    {
        var userId = User.GetId();

        var farmerId = await _serviceManager.FarmerService.GetFarmerByUserIdAsync(Guid.Parse(userId), trackChanges: false);
        //var model = await _serviceManager.OrderService.GetOrderConfirmationForFarmersViewModelAsync(farmerId, trackChanges: false);
        return View();
    }
}
