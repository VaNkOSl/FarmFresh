using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmFresh.Data;
using FarmFresh.Data.Models;
using Microsoft.AspNetCore.Authorization;
namespace FarmFresh.Controllers;

[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    private readonly FarmFreshDbContext _context;


    public AdminController(FarmFreshDbContext context)
    {
        _context = context;
    }

    // GET: Pending Farmers
    public async Task<IActionResult> PendingFarmers()
    {
        var farmers = await _context.Farmers
                                    .Where(f => !f.IsApproved)
                                    .ToListAsync();
        return View(farmers);
    }

    // POST: Approve Farmer
    [HttpPost]
    public async Task<IActionResult> ApproveFarmer(Guid id)
    {
        var farmer = await _context.Farmers.FindAsync(id);
        if (farmer == null)
        {
            return NotFound();
        }

        farmer.IsApproved = true;
        _context.Farmers.Update(farmer);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(PendingFarmers));
    }

    [HttpPost]
    public async Task<IActionResult> RejectFarmer(Guid id)
    {
        var farmer = await _context.Farmers.FindAsync(id);
        if (farmer == null)
        {
            return NotFound();
        }

        _context.Farmers.Remove(farmer);
        await _context.SaveChangesAsync();

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
            return RedirectToAction(nameof(ManageProducts));
        }
        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Farmers = _context.Farmers.Where(f => f.IsApproved).ToList();
        return View(product);
    }
    public async Task<IActionResult> EditProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
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
            return RedirectToAction(nameof(ManageProducts));
        }
        ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Farmers = _context.Farmers.Where(f => f.IsApproved).ToList();
        return View(product);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
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
            return NotFound();
        }

        // Set LockoutEnd to a future date to block the user
        user.LockoutEnd = DateTime.UtcNow.AddYears(100); // A far future date
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(ManageUsers));
    }

   [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UnblockUser(Guid id)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
    {
        return NotFound();
    }

    // Remove the LockoutEnd date to unblock the user
    user.LockoutEnd = null;
    _context.Users.Update(user);
    await _context.SaveChangesAsync();

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
            return RedirectToAction(nameof(ManageCategories));
        }
        return View(category);
    }
    public async Task<IActionResult> EditCategory(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
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
            return RedirectToAction(nameof(ManageCategories));
        }
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
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
            return RedirectToAction("Details", "Product", new { id = review.ProductId });
        }
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


}
