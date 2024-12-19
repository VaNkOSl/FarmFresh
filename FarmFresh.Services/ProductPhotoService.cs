using AutoMapper;
using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts;
using LoggerService.Contacts;
using LoggerService.Exceptions.InternalError.ProductPhotos;
using LoggerService.Exceptions.NotFound;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services;

public sealed class ProductPhotoService : IProductPhotoService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public ProductPhotoService(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager,
        IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task DeleteProductPhotoAsync(Guid photoId, bool trackChanges)
    {
        var productPhotosForDeleting = await
          _repositoryManager
          .ProductPhotoRepository
          .FindProductPhotoByConditionAsync(ph => ph.Id == photoId, trackChanges)
          .ToListAsync();

        if (productPhotosForDeleting is null)
        {
            _loggerManager.LogError($"[{nameof(DeleteProductPhotoAsync)}] Photo with Id {photoId} was not found at Date: {DateTime.UtcNow}");
            throw new ProductPhotoIdNotFoundException(photoId); ;
        }

        try
        {
            foreach (var photo in productPhotosForDeleting)
            {
                _repositoryManager.ProductPhotoRepository.DeleteProductPhoto(photo);
            }

            await _repositoryManager.SaveAsync();
            _loggerManager.LogInfo($"Successfully with ID {photoId}");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new DeleteProductPhotosException(photoId);
        }
    }

    public async Task UploadProductPhotosAsync(ICollection<IFormFile> photos, string uploadDirectory, Product product)
    {
        var productPhotos = new List<ProductPhoto>();

        if (!Directory.Exists(uploadDirectory))
        {
            Directory.CreateDirectory(uploadDirectory);
        }

        foreach (var photo in photos)
        {
            var fileNameWithoutExtension = Path.GetFileName(photo.FileName);
            var extension = Path.GetExtension(photo.FileName);
            var newFileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadDirectory, newFileName);

            if (File.Exists(filePath))
            {
                continue;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            productPhotos.Add(new ProductPhoto
            {
                FilePath = "/uploads/" + newFileName,
                ProductId = product.Id
            });
        }

        if (productPhotos.Any())
        {
            foreach (var photo in productPhotos)
            {
                await _repositoryManager.ProductPhotoRepository.CreateProductPhotoAsync(photo);
            }

            await _repositoryManager.SaveAsync();
            _loggerManager.LogInfo($"[{nameof(UploadProductPhotosAsync)}] Successfully create photos for produc with Id {product.Id} at Date: {DateTime.UtcNow}.");
        }
    }
}
