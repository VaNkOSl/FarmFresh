using FarmFresh.Data.Models.Repositories;
using FarmFresh.Data.Models;
using FarmFresh.Services.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmFresh.ViewModels.Products;

namespace FarmFresh.Services
{
    using FarmFresh.Data.Models.Repositories;
    using FarmFresh.Services.Contacts;
    using FarmFresh.ViewModels;
    using System.IO;

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return products.Select(MapToViewModel);
        }

        public async Task<ProductViewModel?> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return product == null ? null : MapToViewModel(product);
        }

        public async Task<PagedResult<ProductViewModel>> GetPagedProductsAsync(string? filter, int pageIndex, int pageSize)
        {
            var products = await _productRepository.GetPagedProductsAsync(pageIndex, pageSize, filter);
            var totalCount = await _productRepository.GetTotalCountAsync(filter);

            return new PagedResult<ProductViewModel>
            {
                Items = products.Select(MapToViewModel),
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task AddProductAsync(CreateProductViewModel model)
        {
            // Convert uploaded photo to byte[]
            if (model.UploadedPhoto != null && model.UploadedPhoto.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.UploadedPhoto.CopyToAsync(memoryStream);
                    model.Photo = memoryStream.ToArray();
                }
            }

            var product = MapToModel(model);
            await _productRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(EditProductViewModel model)
        {
            // Convert uploaded photo to byte[] (if updated)
            if (model.UploadedPhoto != null && model.UploadedPhoto.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.UploadedPhoto.CopyToAsync(memoryStream);
                    model.Photo = memoryStream.ToArray();
                }
            }

            var product = MapToModel(model);
            await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            await _productRepository.DeleteProductAsync(productId);
        }

        private ProductViewModel MapToViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Description = product.Description,
                CategoryName = product.Category.Name,
                CategoryId = product.CategoryId, // Map this
                FarmerName = product.Farmer.User.UserName,
                FarmerId = product.FarmerId, // Map this
                HarvestDate = product.HarvestDate, // Map this
                ExpirationDate = product.ExpirationDate, // Map this
                Photos = product.ProductPhotos.Select(p => p.FilePath).ToList()
            };
        }

        private Product MapToModel(CreateProductViewModel viewModel)
        {
            return new Product
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                StockQuantity = viewModel.StockQuantity,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId,
                FarmerId = viewModel.FarmerId,
                Photo = viewModel.Photo,
                HarvestDate = viewModel.HarvestDate,
                ExpirationDate = viewModel.ExpirationDate
            };
        }

        private Product MapToModel(EditProductViewModel viewModel)
        {
            return new Product
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Price = viewModel.Price,
                StockQuantity = viewModel.StockQuantity,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId,
                FarmerId = viewModel.FarmerId,
                Photo = viewModel.Photo,
                HarvestDate = viewModel.HarvestDate,
                ExpirationDate = viewModel.ExpirationDate
            };
        }
    }

}
