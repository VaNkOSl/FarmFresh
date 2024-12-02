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

        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return product == null ? null : MapToViewModel(product);
        }

        public async Task AddProductAsync(CreateProductViewModel createModel)
        {
            var product = MapToModel(createModel);
            await _productRepository.AddProductAsync(product);
        }

        public async Task UpdateProductAsync(EditProductViewModel editModel)
        {
            var product = MapToModel(editModel);
            await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
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
                Quantity = product.Quantity,
                Description = product.Description,
                CategoryName = product.Category?.Name
            };
        }

        private Product MapToModel(CreateProductViewModel viewModel)
        {
            return new Product
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                Quantity = viewModel.Quantity,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId
            };
        }

        private Product MapToModel(EditProductViewModel viewModel)
        {
            return new Product
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Price = viewModel.Price,
                Quantity = viewModel.Quantity,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId
            };
        }
    }

}
