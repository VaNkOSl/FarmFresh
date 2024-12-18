using FarmFresh.Infrastructure.Extensions;
using FarmFresh.Services.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using static FarmFresh.Commons.MessagesConstants.Cars;
using static FarmFresh.Commons.NotificationMessagesConstants;

namespace FarmFresh.Controllers;

[Authorize]
[Route("api/cart")]
public class CartController : BaseController
{
    private readonly IServiceManager _serviceManager;

    public CartController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("index")]
    public async Task<IActionResult> Index()
    {
        var userId = User.GetId();
        var cart = await _serviceManager.CartService.GetAllCartItemsAsync(Guid.Parse(userId), trackChanges: false);
        return View(cart);
    }

    [HttpPost("addtocart")]
    public async Task<IActionResult> AddToCart(Guid Id, int quantity)
    {
        var userId = User.GetId();
        var success = await _serviceManager.CartService.AddToCartAsync(Guid.Parse(userId), Id, quantity, trackChanges: true);

        if (!success)
        {
            return BadRequest("Product not found or insufficient stock.");
        }
        TempData[SuccessMessage] = SuccessfullyAddProductToCart;
        return RedirectToAction(nameof(ProductController.Details), "Product", new { id = Id });
    }

    [HttpPost("removefromcart/{productId}")]
    public async Task<IActionResult> RemoveFromCart(Guid productId)
    {
        await _serviceManager.CartService.RemoveFromCart(productId, trackChanges: true);
        TempData[WarningMessage] = SuccessfullyRemoveProductFromCart;
        return RedirectToAction("Index");
    }

    [HttpPost("increasequantity/{productId}")]
    public async Task<IActionResult> IncreaseQuantity(Guid productId)
    {
        var success = await _serviceManager.CartService.UpdateCartQuantityAsync(productId, 1, trackChanges: true);

        if (!success)
        {
            return BadRequest("Insufficient stock.");
        }

        return RedirectToAction("Index");
    }

    [HttpPost("decreasequantity/{productId}")]
    public async Task<IActionResult> DecreaseQuantity(Guid productId)
    {
        var success = await _serviceManager.CartService.UpdateCartQuantityAsync(productId, -1, trackChanges: true);

        if (!success)
        {
            return BadRequest("Quantity cannot be decreased further.");
        }

        return RedirectToAction("Index");
    }
}

public static class SessionExtension
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}
