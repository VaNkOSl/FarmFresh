using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.DataValidator;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace IntegrationTests.ProductTests;

public class ProductRepositoryTests
{
    private readonly FarmFreshDbContext _context;
    private readonly IRepositoryManager _repositoryManager;

    public ProductRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FarmFreshDbContext>()
            .UseInMemoryDatabase("FarmFreshInMemoryDataBase" + Guid.NewGuid())
            .Options;

        _context = new FarmFreshDbContext(options);

        var serviceProviderMock = new Mock<IServiceProvider>();

        var crudDataValidator = new CRUDDataValidator();

        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IValidateEntity)))
            .Returns(crudDataValidator);

        _repositoryManager = new RepositoryManager(_context, serviceProviderMock.Object);
    }

    [Fact]
    public async void CreateProduct_Should_Create_Product_Successfully()
    {
        var initialCount = await _repositoryManager.ProductRepository
            .FindAllProducts(trackChanges: false)
            .CountAsync();

        Assert.Equal(0, initialCount);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Description = "Test Description",
            Price = 100.00M
        };

        await _repositoryManager.ProductRepository.CreateProductAsync(product);
        await _repositoryManager.SaveAsync();

        var realCount = await _repositoryManager.ProductRepository
            .FindAllProducts(trackChanges: false)
            .CountAsync();

        Assert.Equal(initialCount + 1, realCount);
    }

    [Fact]
    public async void GetProductById_Should_Return_Correct_Product()
    {
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Test Product",
            Description = "Test Description",
            Price = 100.00M
        };

        await _repositoryManager.ProductRepository.CreateProductAsync(product);
        await _repositoryManager.SaveAsync();

        var retrievedProduct = await _repositoryManager.ProductRepository
            .GetProductByIdAsync(productId);

        Assert.NotNull(retrievedProduct);
        Assert.Equal(product.Name, retrievedProduct.Name);
        Assert.Equal(product.Description, retrievedProduct.Description);
        Assert.Equal(product.Price, retrievedProduct.Price);
    }

    [Fact]
    public async void UpdateProduct_Should_Update_Product_Successfully()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Description = "Test Description",
            Price = 100.00M
        };

        await _repositoryManager.ProductRepository.CreateProductAsync(product);
        await _repositoryManager.SaveAsync();

        product.Name = "Updated Product Name";
        product.Description = "Updated Description";
        product.Price = 150.00M;

        _repositoryManager.ProductRepository.UpdateProduct(product);
        await _repositoryManager.SaveAsync();

        var updatedProduct = await _repositoryManager.ProductRepository
            .GetProductByIdAsync(product.Id);

        Assert.NotNull(updatedProduct);
        Assert.Equal("Updated Product Name", updatedProduct.Name);
        Assert.Equal("Updated Description", updatedProduct.Description);
        Assert.Equal(150.00M, updatedProduct.Price);
    }

    [Fact]
    public async void DeleteProduct_Should_Delete_Product_Successfully()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Description = "Test Description",
            Price = 100.00M
        };

        await _repositoryManager.ProductRepository.CreateProductAsync(product);
        await _repositoryManager.SaveAsync();

        var countBeforeDeleting = await _repositoryManager.ProductRepository
            .FindAllProducts(trackChanges: false)
            .CountAsync();

        Assert.Equal(1, countBeforeDeleting);

        _repositoryManager.ProductRepository.DeleteProduct(product);
        await _repositoryManager.SaveAsync();

        var countAfterDeleting = await _repositoryManager.ProductRepository
            .FindAllProducts(trackChanges: false)
            .CountAsync();

        Assert.Equal(0, countAfterDeleting);
    }
}
