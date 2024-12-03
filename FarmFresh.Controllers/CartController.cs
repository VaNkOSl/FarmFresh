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

namespace FarmFresh.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";
        private readonly ISession session;
        public IActionResult Index()
        {
            var cart = session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            return View(cart);
        }
        public IActionResult AddToCart(Guid productId, string ProductName, decimal price)
        {
            var cart = session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
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
                    Price = price,
                    Quantity = 1
                });
            }
            session.Set(CartSessionKey, cart);
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
