using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using Microsoft.AspNetCore.Authorization;
using FarmFresh.ViewModels.Admin;
namespace FarmFresh.Controllers;

[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    private readonly FarmFreshDbContext _context;


    public AdminController(FarmFreshDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> PendingFarmers()
    {
        var farmers = await _context.Farmers
                                    .Where(f => !f.IsApproved)
                                    .ToListAsync();
        return View(farmers);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveFarmer(Guid id)
    {
        var farmer = await _context.Farmers.FindAsync(id);
        if (farmer == null)
        {
            TempData["Error"] = "Farmer not found.";
            return RedirectToAction(nameof(PendingFarmers));
        }

        farmer.IsApproved = true;
        _context.Farmers.Update(farmer);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Farmer approved successfully.";
        return RedirectToAction(nameof(PendingFarmers));
    }

    [HttpPost]
    public async Task<IActionResult> RejectFarmer(Guid id)
    {
        var farmer = await _context.Farmers.FindAsync(id);
        if (farmer == null)
        {
            TempData["Error"] = "Farmer not found.";
            return RedirectToAction(nameof(PendingFarmers));
        }

        _context.Farmers.Remove(farmer);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Farmer rejected successfully.";
        return RedirectToAction(nameof(PendingFarmers));
    }
    public async Task<IActionResult> ManageProducts()
    {
        var products = await _context.Products
                                     .Include(p => p.Farmer)
                                     .Include(p => p.Category)
                                     .ToListAsync();
        return View(products);
    }
    public IActionResult AddProduct()
    {
        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Farmers = _context.Farmers.Where(f => f.IsApproved).ToList();
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> AddProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            product.Id = Guid.NewGuid();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product added successfully.";
            return RedirectToAction(nameof(ManageProducts));
        }
        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Farmers = _context.Farmers.Where(f => f.IsApproved).ToList();
        TempData["Error"] = "Failed to add product. Please check the form.";
        return View(product);
    }

    public async Task<IActionResult> EditProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            TempData["Error"] = "Product not found.";
            return RedirectToAction(nameof(ManageProducts));
        }
        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Farmers = _context.Farmers.Where(f => f.IsApproved).ToList();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product updated successfully.";
            return RedirectToAction(nameof(ManageProducts));
        }
        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Farmers = _context.Farmers.Where(f => f.IsApproved).ToList();
        TempData["Error"] = "Failed to update product. Please check the form.";
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            TempData["Error"] = "Product not found.";
            return RedirectToAction(nameof(ManageProducts));
        }
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Product deleted successfully.";
        return RedirectToAction(nameof(ManageProducts));
    }

    public async Task<IActionResult> ManageUsers()
    {
        var users = await _context.Users
                                  .Select(u => new
                                  {
                                      u.Id,
                                      u.UserName,
                                      u.Email,
                                      IsBlocked = u.IsBlocked // Assuming 'IsBlocked' is a property in your User model
                                  })
                                  .ToListAsync();

        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BlockUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            TempData["Error"] = "User not found.";
            return RedirectToAction(nameof(ManageUsers));
        }

        // Set LockoutEnd to a future date to block the user
        user.LockoutEnd = DateTime.UtcNow.AddYears(100); // A far future date
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        TempData["Success"] = "User blocked successfully.";
        return RedirectToAction(nameof(ManageUsers));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnblockUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            TempData["Error"] = "User not found.";
            return RedirectToAction(nameof(ManageUsers));
        }

        // Remove the LockoutEnd date to unblock the user
        user.LockoutEnd = null;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        TempData["Success"] = "User unblocked successfully.";
        return RedirectToAction(nameof(ManageUsers));
    }

    public async Task<IActionResult> ManageCategories()
    {
        var categories = await _context.Categories.ToListAsync();
        return View(categories);
    }
    public IActionResult AddCategory()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            category.Id = Guid.NewGuid();
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category added successfully.";
            return RedirectToAction(nameof(ManageCategories));
        }

        TempData["Error"] = "Failed to add category.";
        return View(category);
    }
    public async Task<IActionResult> EditCategory(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            TempData["Error"] = "Category not found.";
            return RedirectToAction(nameof(ManageCategories));
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction(nameof(ManageCategories));
        }
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            TempData["Error"] = "Category not found.";
            return RedirectToAction(nameof(ManageCategories));
        }
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Category deleted successfully.";
        return RedirectToAction(nameof(ManageCategories));
    }

    public IActionResult AddReview(Guid productId)
    {
        ViewBag.ProductId = productId;
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddReview(Review review)
    {
        if (ModelState.IsValid)
        {
            review.Id = Guid.NewGuid();
            review.ReviewDate = DateTime.UtcNow;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Review added successfully.";
            return RedirectToAction("Details", "Product", new { id = review.ProductId });
        }
        TempData["Error"] = "Failed to add review.";
        ViewBag.ProductId = review.ProductId;
        return View(review);
    }

    [HttpGet]
    public async Task<IActionResult> ManageAllProducts()
    {
        var allProducts = await _context.Products
            .Include(p => p.Farmer)
            .Include(p => p.Category)
            .ToListAsync();

        return View(allProducts);
    }
    public IActionResult AdminDashboard()
    {
        return View();
    }
    public async Task<IActionResult> AdminReports()
    {
        var totalUsers = await _context.Users.CountAsync();
        var totalProducts = await _context.Products.CountAsync();
        var totalFarmers = await _context.Farmers.CountAsync();

        // Correct the projection to match the TopProductViewModel
        var topProducts = await _context.Products
                                        .OrderByDescending(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0) // Check for no reviews
                                        .Take(5)
                                        .Select(p => new TopProductViewModel
                                        {
                                            Name = p.Name,
                                            AverageRating = p.Reviews.Any()
                                                ? p.Reviews.Average(r => r.Rating)
                                                : 0 // Default rating if no reviews
                                        })
                                        .ToListAsync();

        // Create the view model
        var model = new AdminReportViewModel
        {
            TotalUsers = totalUsers,
            TotalProducts = totalProducts,
            TotalFarmers = totalFarmers,
            TopProducts = topProducts
        };

        TempData["Info"] = "Report generated successfully.";
        return View(model);
    }

}
