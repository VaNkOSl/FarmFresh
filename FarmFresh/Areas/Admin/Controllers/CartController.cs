using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FarmFresh.Data.Models;
using FarmFresh.ViewModels.Order;
using Microsoft.AspNetCore.Http;

using System.Text.Json;
using FarmFresh.Data;
using Microsoft.AspNetCore.Authorization;
using FarmFresh.Services.Contacts;

namespace FarmFresh.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";
        private readonly FarmFreshDbContext _context;
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            return View(cart);
        }
        public async Task<IActionResult> AddToCart(Guid productId)
        {
            var success = await _cartService.AddToCartAsync(productId, CartSessionKey, HttpContext.Session);

            if (!success)
            {
                return BadRequest("Product not found or insufficient stock.");
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(Guid productId)
        {
            _cartService.RemoveFromCart(productId, CartSessionKey, HttpContext.Session);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> IncreaseQuantity(Guid productId)
        {
            var success = await _cartService.UpdateCartQuantityAsync(productId, 1, CartSessionKey, HttpContext.Session);

            if (!success)
            {
                return BadRequest("Insufficient stock.");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DecreaseQuantity(Guid productId)
        {
            var success = await _cartService.UpdateCartQuantityAsync(productId, -1, CartSessionKey, HttpContext.Session);

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
}
