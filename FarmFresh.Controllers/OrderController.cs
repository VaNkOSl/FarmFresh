using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.ViewModels.Order;
using FarmFresh.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static FarmFresh.Commons.EntityValidationConstants;

namespace FarmFresh.Controllers
{
    public class OrderController : Controller
    {
        private readonly FarmFreshDbContext _context;

        public OrderController(FarmFreshDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = _context.OrderProducts
                .Include(o => o.Product)
                .Include(o => o.Order)
                .ThenInclude(o => o.User)
                .Where(o => o.Order.UserId.ToString() == userId)
                .Select(o => new OrderListViewModel
                {
                    Id = o.Id,
                    OrderId = o.OrderId,
                    ProductName = o.Product.Name,
                    OrderStatus = o.Order.OrderStatus.ToString(),
                    Price=o.Price,
                    Quantity = o.Quantity,
                    Picture=o.Product.Photo,
                }).ToList();
            return View(orders);
        }
        //public IActionResult Details(Guid OrderId)
        //{
        //}
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
        public async Task<IActionResult> Checkout(Order order)
        {

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            order.UserId = userId;
            try
            {
                order.OrderStatus = OrderStatus.Cart;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToAction("OrderConfirmation", new { id = order.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
            }


            return View(order);
        }
    }
}
