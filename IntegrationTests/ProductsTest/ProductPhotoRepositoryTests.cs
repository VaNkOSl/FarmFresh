using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.DataValidator;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace IntegrationTests.ProductPhotoTests;

public class ProductPhotoRepositoryTests
{
    private readonly FarmFreshDbContext _context;
    private readonly IProductPhotoRepository _productPhotoRepository;

    public ProductPhotoRepositoryTests()
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

        _productPhotoRepository = new ProductPhotoRepository(_context, crudDataValidator);
    }

    [Fact]
    public async Task CreateProductPhoto_Should_Create_ProductPhoto_Successfully()
    {
        // Arrange
        var initialCount = await _productPhotoRepository
            .FindProductPhotoByConditionAsync(p => true, trackChanges: false)
            .CountAsync();

        var productPhoto = new ProductPhoto
        {
            Id = Guid.NewGuid(),
            FilePath = "/uploads/test-photo.jpg",
            ProductId = Guid.NewGuid()
        };

        // Act
        await _productPhotoRepository.CreateProductPhotoAsync(productPhoto);
        await _context.SaveChangesAsync();

        var realCount = await _productPhotoRepository
            .FindProductPhotoByConditionAsync(p => true, trackChanges: false)
            .CountAsync();

        // Assert
        Assert.Equal(initialCount + 1, realCount);
    }

    [Fact]
    public async Task DeleteProductPhoto_Should_Delete_ProductPhoto_Successfully()
    {
        // Arrange
        var productPhoto = new ProductPhoto
        {
            Id = Guid.NewGuid(),
            FilePath = "/uploads/test-photo.jpg",
            ProductId = Guid.NewGuid()
        };

        await _productPhotoRepository.CreateProductPhotoAsync(productPhoto);
        await _context.SaveChangesAsync();

        var countBeforeDeleting = await _productPhotoRepository
            .FindProductPhotoByConditionAsync(p => true, trackChanges: false)
            .CountAsync();

        // Act
        _productPhotoRepository.DeleteProductPhoto(productPhoto);
        await _context.SaveChangesAsync();

        var countAfterDeleting = await _productPhotoRepository
            .FindProductPhotoByConditionAsync(p => true, trackChanges: false)
            .CountAsync();

        // Assert
        Assert.Equal(countBeforeDeleting - 1, countAfterDeleting);
    }

    [Fact]
    public async Task UpdateProductPhoto_Should_Update_ProductPhoto_Successfully()
    {
        // Arrange
        var productPhoto = new ProductPhoto
        {
            Id = Guid.NewGuid(),
            FilePath = "/uploads/test-photo.jpg",
            ProductId = Guid.NewGuid()
        };

        await _productPhotoRepository.CreateProductPhotoAsync(productPhoto);
        await _context.SaveChangesAsync();

        productPhoto.FilePath = "/uploads/updated-photo.jpg";

        // Act
        _productPhotoRepository.UpdateProductPhoto(productPhoto);
        await _context.SaveChangesAsync();

        var updatedPhoto = await _productPhotoRepository
            .FindProductPhotoByConditionAsync(p => p.Id == productPhoto.Id, trackChanges: false)
            .FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(updatedPhoto);
        Assert.Equal("/uploads/updated-photo.jpg", updatedPhoto.FilePath);
    }

    [Fact]
    public async Task FindProductPhotoByConditionAsync_Should_Return_Correct_ProductPhoto()
    {
        // Arrange
        var productPhotoId = Guid.NewGuid();
        var productPhoto = new ProductPhoto
        {
            Id = productPhotoId,
            FilePath = "/uploads/test-photo.jpg",
            ProductId = Guid.NewGuid()
        };

        await _productPhotoRepository.CreateProductPhotoAsync(productPhoto);
        await _context.SaveChangesAsync();

        // Act
        var retrievedPhoto = await _productPhotoRepository
            .FindProductPhotoByConditionAsync(p => p.Id == productPhotoId, trackChanges: false)
            .FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(retrievedPhoto);
        Assert.Equal(productPhoto.FilePath, retrievedPhoto.FilePath);
    }
}
