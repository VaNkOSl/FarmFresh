using FarmFresh.Data;
using FarmFresh.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace FarmFresh.Controllers
{
    public class ProductController : Controller
    {
        private readonly FarmFreshDbContext _context;

        public ProductController(FarmFreshDbContext context)
        {
            _context = context;
        }

        // GET: All Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                                         .Include(p => p.Farmer)
                                         .Include(p => p.Category)
                                         .Include(p => p.ProductPhotos)
                                         .ToListAsync();
            return View(products);
        }

        // GET: Add Product Form
        public IActionResult Add()
        {
            return View();
        }

        // POST: Add Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product, List<IFormFile> uploadedPhotos)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid(); // Generate a new unique ID for the product

                // Handle uploaded photos
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

        // GET: Edit Product Form
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

        // POST: Edit Product
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

                    // Update product properties
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

                    // Handle new photos
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

        // GET: Product Details
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _context.Products
                                        .Include(p => p.Farmer)
                                        .Include(p => p.Category)
                                        .Include(p => p.ProductPhotos)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Delete Product
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

            // Remove associated photos
            _context.ProductPhotos.RemoveRange(product.ProductPhotos);

            // Remove the product
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
