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

namespace FarmFresh.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";
        private FarmFreshDbContext _context;
        public CartController(FarmFreshDbContext context)
        {
            _context= context;
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            return View(cart);
        }
        public IActionResult AddToCart(Guid productId, string ProductName, decimal price)
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);

            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            int requestedQuantity = existingItem != null ? existingItem.Quantity + 1 : 1;
            if (product.StockQuantity < requestedQuantity)
            {
                return BadRequest("Not enough stock available.");
            }

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItemViewModel
                {
                    ProductId = productId,
                    ProductName = ProductName,
                    Quantity = 1,
                    Price = price
                });
            }

            HttpContext.Session.Set(CartSessionKey, cart);

            return RedirectToAction("Index");
        }
        public IActionResult RemoveFromCart(Guid ProductId)
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            if (cart != null)
            {
                var item = cart.FirstOrDefault(i => i.ProductId == ProductId);
                if(item != null)
                {
                    cart.Remove(item);
                    HttpContext.Session.Set(CartSessionKey, cart);
                }
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
