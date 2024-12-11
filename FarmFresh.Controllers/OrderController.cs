using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Order;
using FarmFresh.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static FarmFresh.Commons.EntityValidationConstants;

namespace FarmFresh.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await _orderService.GetOrdersForUserAsync(userId);
            return View(orders);
        }
        public async Task<IActionResult> Details(Guid Id)
        {
            var orderDetails = await _orderService.GetOrderDetailsAsync(Id);

            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }
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
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order, Guid OrderProductId)
        {

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var orderId = await _orderService.CheckoutAsync(order, OrderProductId, userId);
                return RedirectToAction("OrderConfirmation", new { id = orderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
            }


            return View(order);
        }
        public async Task<IActionResult> OrderConfirmation(Guid id)
        {
            var orderViewModel = await _orderService.GetOrderConfirmationViewModelAsync(id);

            if (orderViewModel == null)
            {
                return NotFound();
            }

            return View(orderViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderConfirmation(Order order)
        {
            try
            {
                await _orderService.CompleteOrderAsync(order.Id);
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
    }
}
