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
    [Authorize]
    public class OrderController : Controller
    {
        private readonly FarmFreshDbContext _context;

        public OrderController(FarmFreshDbContext context)
        {
            _context = context;
        }
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
        public IActionResult Details(Guid Id)
        {
            var OrderDetails=_context.OrderProducts
                .Where(o => o.Id == Id)
                .Include(o => o.Product)
                .ThenInclude(o => o.Farmer)
                .ThenInclude(o => o.User)
                .Include(o => o.Order)
                .Select(o => new OrderDetailsViewModel
                {
                    Id = o.OrderId,
                    CreatedDate = o.Order.CreateOrderdDate,
                    OrderId = o.OrderId,
                    Quantity = o.Quantity,
                    Price = o.Price,
                    FirstName = o.Order.FirstName,
                    LastName = o.Order.LastName,
                    Adress = o.Order.Adress,
                    PhoneNumber = o.Order.PhoneNumber,
                    Email = o.Order.Email,
                    ProductName = o.Product.Name,
                    DeliveryOption = o.Order.DeliveryOption,
                    OrderStatus = o.Order.OrderStatus.ToString(),
                    ProductDescription = o.Product.Description,
                    FarmerName = o.Product.Farmer.User.FirstName + " " + o.Product.Farmer.User.LastName,
                    Origin = o.Product.Origin,
                    ProductPrice = o.Product.Price,
                    Seasons = o.Product.SuitableSeason,
                    HarvestDate = o.Product.HarvestDate,
                    ExpirationDate = o.Product.ExpirationDate,
                    Picture=o.Product.Photo
                });
            return View(OrderDetails);
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
        public IActionResult OrderConfirmation(Guid id)
        {
            {
                var order = _context.Orders
                    .Where(o => o.Id == id)
                    .Select(o => new OrderConfirmationViewModel
                    {
                        Id = o.Id,
                        OrderId = o.Id,
                        Products = o.OrderProducts.ToList(),
                        Price = o.OrderProducts.Sum(p => p.Price), 
                        Quantity = o.OrderProducts.Sum(p => p.Quantity),
                        TotalPrice = o.OrderProducts.Sum(p => p.Price * p.Quantity),
                        Picture = o.OrderProducts.FirstOrDefault().Product.Photo,   
                        FirstName = o.FirstName, 
                        LastName = o.LastName,
                        Adress = o.Adress,
                        PhoneNumber = o.PhoneNumber,
                        Email = o.Email
                    }).FirstOrDefault();

                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderConfirmation(Order order)
        {
            if (order == null)
            {
                return NotFound();
            }
            order.OrderStatus = OrderStatus.Completed;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
