using FarmFresh.Data;
using FarmFresh.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;

namespace FarmFresh.Controllers
{
    public class ProductController : Controller
    {
        private readonly FarmFreshDbContext _context;

        public ProductController(FarmFreshDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                                         .Include(p => p.Farmer)
                                         .Include(p => p.Category)
                                         .Include(p => p.ProductPhotos)
                                         .ToListAsync();
            return View(products);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product, List<IFormFile> uploadedPhotos)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid(); 

                foreach (var file in uploadedPhotos)
                {
                    if (file.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);

                        var productPhoto = new ProductPhoto
                        {
                            Id = Guid.NewGuid(),
                            Photo = memoryStream.ToArray(),
                            FilePath = Path.GetFileName(file.FileName),
                            ProductId = product.Id
                        };

                        product.ProductPhotos.Add(productPhoto);
                    }
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _context.Products
                                        .Include(p => p.ProductPhotos)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product updatedProduct, List<IFormFile> uploadedPhotos)
        {
            if (id != updatedProduct.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Products
                                                        .Include(p => p.ProductPhotos)
                                                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    existingProduct.Name = updatedProduct.Name;
                    existingProduct.Description = updatedProduct.Description;
                    existingProduct.Origin = updatedProduct.Origin;
                    existingProduct.Price = updatedProduct.Price;
                    existingProduct.StockQuantity = updatedProduct.StockQuantity;
                    existingProduct.SuitableSeason = updatedProduct.SuitableSeason;
                    existingProduct.HarvestDate = updatedProduct.HarvestDate;
                    existingProduct.ExpirationDate = updatedProduct.ExpirationDate;
                    existingProduct.FarmerId = updatedProduct.FarmerId;
                    existingProduct.CategoryId = updatedProduct.CategoryId;

                    foreach (var file in uploadedPhotos)
                    {
                        if (file.Length > 0)
                        {
                            using var memoryStream = new MemoryStream();
                            await file.CopyToAsync(memoryStream);

                            var productPhoto = new ProductPhoto
                            {
                                Id = Guid.NewGuid(),
                                Photo = memoryStream.ToArray(),
                                FilePath = Path.GetFileName(file.FileName),
                                ProductId = existingProduct.Id
                            };

                            existingProduct.ProductPhotos.Add(productPhoto);
                        }
                    }

                    _context.Products.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(p => p.Id == id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(updatedProduct);
        }

        public async Task<IActionResult> Details(Guid id, int page = 1, int pageSize = 5)
        {
            var product = await _context.Products
                .Include(p => p.Farmer)
                .Include(p => p.Category)
                .Include(p => p.ProductPhotos)
                .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var paginatedReviews = product.Reviews
                .OrderByDescending(r => r.ReviewDate) // Sort reviews by the most recent
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(product.Reviews.Count() / (double)pageSize);

            var recommendedProducts = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != id && p.IsApproved) // Same category, exclude current product
                .OrderByDescending(p => p.StockQuantity) // Order by stock or other criteria
                .Take(4) // Limit recommendations to 4
                .Include(p => p.ProductPhotos) // Include photos for recommendations
                .ToListAsync();

            ViewBag.RecommendedProducts = recommendedProducts;
            ViewBag.PaginatedReviews = paginatedReviews;

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _context.Products
                                        .Include(p => p.ProductPhotos)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.ProductPhotos.RemoveRange(product.ProductPhotos);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> PendingApproval()
        {
            var unapprovedProducts = await _context.Products
                                                   .Where(p => !p.IsApproved)
                                                   .Include(p => p.Farmer)
                                                   .ToListAsync();
            return View(unapprovedProducts);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.IsApproved = true;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(PendingApproval));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(PendingApproval));
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var totalProducts = await _context.Products.CountAsync();
            var products = await _context.Products
                                         .Where(p => p.IsApproved) // Only approved products
                                         .OrderBy(p => p.Name)
                                         .Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            ViewBag.CurrentPage = page;

            return View(products);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddReview(Guid id, int rating, string content)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            var review = new Review
            {
                Rating = rating,
                Content = content,
                ProductId = id,
                UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), // Assuming user authentication
                ReviewDate = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id });
        }
        [HttpGet]
        public async Task<IActionResult> Index(
     int page = 1,
     int pageSize = 10,
     string searchName = null,
     decimal? minPrice = null,
     decimal? maxPrice = null,
     string sortOrder = null,
     string category = null)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            ViewBag.SelectedCategory = category;
            ViewBag.SortOrder = sortOrder;

            var query = _context.Products
                                .Where(p => p.IsApproved) // Only show approved products
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(p => p.Name.Contains(searchName));
            }
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.CategoryId.ToString() == category);
            }

            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "rating_desc" => query.OrderByDescending(p => p.Reviews.Average(r => r.Rating)),
                _ => query.OrderBy(p => p.Name) // Default: Alphabetical
            };

            var totalProducts = await query.CountAsync();
            var products = await query.Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            ViewBag.FeaturedProducts = await GetFeaturedProductsAsync();

            ViewBag.TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            ViewBag.CurrentPage = page;

            return View(products);
        }

        [HttpPost]
public async Task<IActionResult> PlaceOrder(Guid id, int quantity, Order orderInput)
{
    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

    if (product == null)
    {
        return NotFound("Product not found."); 
    }

    if (quantity <= 0)
    {
        return BadRequest("Quantity must be greater than zero."); // Handle invalid quantity
    }

    if (product.StockQuantity < quantity)
    {
        return BadRequest($"Only {product.StockQuantity} units available for this product."); // Provide specific stock information
    }

    product.StockQuantity -= quantity;

    var order = new Order
    {
        Id = Guid.NewGuid(), 
        FirstName = orderInput.FirstName,
        LastName = orderInput.LastName,
        Adress = orderInput.Adress,
        PhoneNumber = orderInput.PhoneNumber,
        Email = orderInput.Email,
        DeliveryOption = orderInput.DeliveryOption,
        CreateOrderdDate = DateTime.UtcNow,
        UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), // Assuming user is logged in
        IsTaken = false,
        OrderStatus = null // Initial status
    };

    var orderProduct = new OrderProduct
    {
        OrderId = order.Id,
        ProductId = id,
        Quantity = quantity
    };

    order.OrderProducts = new List<OrderProduct> { orderProduct };

    try
    {
        _context.Products.Update(product);
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
        return BadRequest($"An error occurred while processing your order: {ex.Message}");
    }

    return RedirectToAction("Details", new { id });
}

        [HttpGet]
        public async Task<IActionResult> EditReview(Guid reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review == null) return NotFound();

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(Guid id, string content, int rating)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(content) || rating < 1 || rating > 5)
            {
                ModelState.AddModelError("", "Invalid data provided.");
                return View();
            }

            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            // Update the review
            review.Content = content;
            review.Rating = rating;

            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = review.ProductId });
        }


        // POST: Delete Review
        [HttpPost]
        public async Task<IActionResult> DeleteReview(Guid reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review == null) return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = review.ProductId });
        }
        public async Task<IActionResult> UpdateStock(Guid productId, int quantitySold)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.StockQuantity < quantitySold)
            {
                return BadRequest("Not enough stock available.");
            }

            product.StockQuantity -= quantitySold;

            if (product.StockQuantity == 0)
            {
                product.IsAvailable = false; // Mark product as unavailable
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Ok("Stock updated successfully.");
        }
        [HttpGet]
        public async Task<IActionResult> SearchSuggestions(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Json(new List<string>());
            }

            var suggestions = await _context.Products
                                             .Where(p => p.Name.Contains(term))
                                             .Select(p => p.Name)
                                             .Take(10)
                                             .ToListAsync();

            return Json(suggestions);
        }
        public async Task<List<Product>> GetFeaturedProductsAsync()
        {
            return await _context.Products
                                 .Where(p => p.IsApproved && p.StockQuantity > 0)
                                 .OrderByDescending(p => p.Price) // Sort by highest price
                                 .Take(5) // Limit to 5 products
                                 .Include(p => p.ProductPhotos)
                                 .ToListAsync();
        }
        [HttpPost]
        public async Task<IActionResult> AddToWishlist(Guid productId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var existingWishlistItem = await _context.Wishlists
                                                     .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (existingWishlistItem == null)
            {
                var wishlistItem = new Wishlist
                {
                    UserId = userId,
                    ProductId = productId
                };

                _context.Wishlists.Add(wishlistItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ViewWishlist()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var wishlist = await _context.Wishlists
                                         .Where(w => w.UserId == userId)
                                         .Include(w => w.Product)
                                         .ThenInclude(p => p.ProductPhotos)
                                         .ToListAsync();

            return View(wishlist);
        }

    }

}
