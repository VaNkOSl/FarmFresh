using FarmFresh.Data;
using FarmFresh.Data.Models;
using FarmFresh.Repositories;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.DataValidator;
using FarmFresh.Services;
using FarmFresh.Services.Contacts;
using LoggerService.Contacts;
using LoggerService.Exceptions.InternalError.ProductPhotos;
using LoggerService.Exceptions.NotFound;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.IO;
using System.Text;
using Xunit;

namespace IntegrationTests.ProductPhotoTests;

public class ProductPhotoServiceTests
{
    private readonly FarmFreshDbContext _context;
    private readonly IRepositoryManager _repositoryManager;
    private readonly Mock<ILoggerManager> _loggerManagerMock; 
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly ProductPhotoService _productPhotoService;

    public ProductPhotoServiceTests()
    {
        var options = new DbContextOptionsBuilder<FarmFreshDbContext>()
            .UseInMemoryDatabase("FarmFreshInMemoryDatabase" + Guid.NewGuid())
            .Options;

        _context = new FarmFreshDbContext(options);

        _serviceProviderMock = new Mock<IServiceProvider>();
        var crudDataValidator = new CRUDDataValidator();
        _serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IValidateEntity)))
            .Returns(crudDataValidator);

        _repositoryManager = new RepositoryManager(_context, _serviceProviderMock.Object);
        _loggerManagerMock = new Mock<ILoggerManager>();

        _productPhotoService = new ProductPhotoService(_repositoryManager, _loggerManagerMock.Object, null);
    }

    [Fact]
    public async Task DeleteProductPhoto_Should_Delete_Photo_Successfully()
    {
        // Arrange
        var photoId = Guid.NewGuid();
        var productPhoto = new ProductPhoto
        {
            Id = photoId,
            FilePath = "/uploads/test.jpg",
            ProductId = Guid.NewGuid()
        };

        await _repositoryManager.ProductPhotoRepository.CreateProductPhotoAsync(productPhoto);
        await _repositoryManager.SaveAsync();

        // Act
        await _productPhotoService.DeleteProductPhotoAsync(photoId, trackChanges: true);

        var deletedPhoto = await _repositoryManager.ProductPhotoRepository
            .FindProductPhotoByConditionAsync(p => p.Id == photoId, trackChanges: false)
            .FirstOrDefaultAsync();

        // Assert
       Assert.Null(deletedPhoto);
        _loggerManagerMock.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteProductPhoto_Should_Throw_NotFound_Exception()
    {
        // Arrange
        var photoId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<ProductPhotoIdNotFoundException>(() =>
            _productPhotoService.DeleteProductPhotoAsync(photoId, trackChanges: true));
        _loggerManagerMock.Verify(log => log.LogError(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UploadProductPhotos_Should_Upload_Photos_Successfully()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId };
        var uploadDirectory = Path.Combine("wwwroot", "uploads");

        var formFileMock = new Mock<IFormFile>();
        var content = "Fake File Content";
        var fileName = "test.jpg";

        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        formFileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
        formFileMock.Setup(f => f.FileName).Returns(fileName);
        formFileMock.Setup(f => f.Length).Returns(memoryStream.Length);

        var files = new List<IFormFile> { formFileMock.Object };

        // Act
        await _productPhotoService.UploadProductPhotosAsync(files, uploadDirectory, product);

        // Assert
        var photosInDatabase = await _repositoryManager.ProductPhotoRepository
            .FindProductPhotoByConditionAsync(p => p.ProductId == productId, trackChanges: false)
            .ToListAsync();

        Assert.NotEmpty(photosInDatabase);
        Assert.Contains(photosInDatabase, p => p.FilePath.Contains(fileName));
        _loggerManagerMock.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UploadProductPhotos_Should_Handle_Existing_Files()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId };
        var uploadDirectory = Path.Combine("wwwroot", "uploads");

        var formFileMock = new Mock<IFormFile>();
        var content = "Fake File Content";
        var fileName = "existing.jpg";

        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        formFileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
        formFileMock.Setup(f => f.FileName).Returns(fileName);
        formFileMock.Setup(f => f.Length).Returns(memoryStream.Length);

        var files = new List<IFormFile> { formFileMock.Object };

        // Create a file to simulate an existing file
        var existingFilePath = Path.Combine(uploadDirectory, fileName);
        Directory.CreateDirectory(uploadDirectory);
        await File.WriteAllTextAsync(existingFilePath, "Existing content");

        // Act
        await _productPhotoService.UploadProductPhotosAsync(files, uploadDirectory, product);

        // Assert
        var photosInDatabase = await _repositoryManager.ProductPhotoRepository
            .FindProductPhotoByConditionAsync(p => p.ProductId == productId, trackChanges: false)
            .ToListAsync();

        Assert.NotEmpty(photosInDatabase);
        Assert.Contains(photosInDatabase, p => p.FilePath.Contains(fileName));
        _loggerManagerMock.Verify(log => log.LogInfo(It.IsAny<string>()), Times.Once);

        // Cleanup
        File.Delete(existingFilePath);
    }
}
