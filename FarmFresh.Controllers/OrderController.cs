using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Infrastructure.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FarmFresh.Controllers;

[Authorize]
[Route("api/order")]
public class OrderController : BaseController
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

    [HttpGet("details/{Id}")]
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
        var model = await _serviceManager.OrderService.GetOrderConfirmationForFarmersViewModelAsync(farmerId, trackChanges: false);
        return View(model);
    }

    [HttpGet("checkout")]
    public IActionResult Checkout()
    {
        var DeliveryList = Enum.GetValues(typeof(DeliveryOption))
     .Cast<DeliveryOption>()
     .Select(s => new SelectListItem
     {
         Text = s.ToString(),
         Value = ((int)s).ToString()
     })
     .ToList();
        ViewData["DeliveryOption"] = DeliveryList;
        return View();
    }

    [HttpPost("sendorder/{id}")]
    public async Task<IActionResult> SendOrder(Guid id)
    {
        await _serviceManager.OrderService.SendOrderAsync(id, trackChanges: true);
        return RedirectToAction("GetOrderBySeller", "Order");
    }

    [HttpPost("cancelorder")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        await _serviceManager.OrderService.CancelOrder(id, trackChanges: true);
        return RedirectToAction("GetOrderBySeller", "Order");
    }
}
