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

        public async Task AddProductAsync(CreateProductViewModel model)
        {
            var product = MapToModel(model);
            await _productRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(EditProductViewModel model)
        {
            var product = MapToModel(model);
            await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            await _productRepository.DeleteProductAsync(productId);
        }

        public async Task<IEnumerable<ProductViewModel>> GetPagedProductsAsync(int pageIndex, int pageSize, string filter)
        {
            var products = await _productRepository.GetPagedProductsAsync(pageIndex, pageSize, filter);
            return products.Select(MapToViewModel);
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
                FarmerName = product.Farmer.User.UserName, 
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
