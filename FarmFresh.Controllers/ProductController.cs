using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FarmFresh.Controllers
{
        public class ProductController : Controller
        {
            private readonly IProductService _productService;
            private readonly ICategoryService _categoryService;
            private readonly IDropdownService _dropdownService; // For farmer dropdown

            public ProductController(IProductService productService, ICategoryService categoryService, IDropdownService dropdownService)
            {
                _productService = productService;
                _categoryService = categoryService;
                _dropdownService = dropdownService;
            }

            // GET: All Products
            public async Task<IActionResult> Index(string? filter, int pageIndex = 1, int pageSize = 10)
            {
                var products = await _productService.GetPagedProductsAsync(filter, pageIndex, pageSize);
                return View(products);
            }

            // GET: Create Product
            public async Task<IActionResult> Create()
            {
                await PopulateDropdowns();
                return View(new CreateProductViewModel());
            }

            // POST: Create Product
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(CreateProductViewModel model)
            {
                if (ModelState.IsValid)
                {
                    await _productService.AddProductAsync(model);
                    return RedirectToAction(nameof(Index));
                }

                await PopulateDropdowns(); // Re-populate dropdowns if validation fails
                return View(model);
            }

            // GET: Edit Product
            public async Task<IActionResult> Edit(Guid id)
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound();

                await PopulateDropdowns();

                var editModel = new EditProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    FarmerId = product.FarmerId,
                    HarvestDate = product.HarvestDate,
                    ExpirationDate = product.ExpirationDate
                };

                return View(editModel);
            }

            // POST: Edit Product
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(EditProductViewModel model)
            {
                if (ModelState.IsValid)
                {
                    await _productService.UpdateProductAsync(model);
                    return RedirectToAction(nameof(Index));
                }

                await PopulateDropdowns(); // Re-populate dropdowns if validation fails
                return View(model);
            }

            // GET: Product Details
            public async Task<IActionResult> Details(Guid id)
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound();

                return View(product);
            }

            // POST: Delete Product
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Delete(Guid id)
            {
                await _productService.DeleteProductAsync(id);
                return RedirectToAction(nameof(Index));
            }

            // Helper Method to Populate Dropdowns
            private async Task PopulateDropdowns()
            {
                ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Farmers = await _dropdownService.GetAllFarmersForDropdownAsync();
            }
        }
    }

