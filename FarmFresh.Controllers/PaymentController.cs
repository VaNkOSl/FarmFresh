using FarmFresh.Infrastructure.Extensions;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using static FarmFresh.Commons.NotificationMessagesConstants;
using static FarmFresh.Commons.MessagesConstants.Orders;

namespace FarmFresh.Controllers;

[Route("api/payment")]
public class PaymentController : BaseController
{
    private readonly IOptions<StripeSettings> _stripeSettings;
    private readonly IServiceManager _serviceManager;

    public PaymentController(IOptions<StripeSettings> stripeSetting,
                             IServiceManager serviceManager)
    {
        _stripeSettings = stripeSetting;
        _serviceManager = serviceManager;
    }

    [Authorize]
    [HttpGet("processcardpayment/{id}")]
    public async Task<IActionResult> ProcessCardPayment(Guid id)
    {
        var userId = User.GetId();
        var totalSum = await _serviceManager.CartService.GetTotalSumAsync(Guid.Parse(userId), trackChanges: false);
        var model = new PaymentViewModel
        {
            Amount = totalSum,
            OrderId = id
        };
        ViewBag.PublishableKey = _stripeSettings.Value.PublishableKey;
        return View(model);
    }

    [HttpPost("processcardpayment/{id}")]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> ProcessCardPayment(Guid id,PaymentViewModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.StripeToken))
        {
            ModelState.AddModelError("", "Invalid payment information.");
            return View("Index", model);
        }

        var options = new ChargeCreateOptions
        {
            Amount = (long)(model.Amount * 100),
            Currency = "usd",
            Description = "FarmFresh Order",
            Source = model.StripeToken,
        };

        var service = new ChargeService();
        Charge charge = await service.CreateAsync(options);

        await _serviceManager.OrderService.CompleteOrderAsync(id, trackChanges: true);

        if (charge.Status == "succeeded")
        {
            TempData[SuccessMessage] = OrderSuccessfullyPlaced;
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError("", "Payment failed. Please try again.");
            return View("Index", model);
        }
    }
}
