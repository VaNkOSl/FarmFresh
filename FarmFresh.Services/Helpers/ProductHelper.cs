using FarmFresh.Data.Models;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Repositories.Extensions;
using LoggerService;
using LoggerService.Contacts;
using LoggerService.Exceptions.InternalError.ProductPhotos;
using LoggerService.Exceptions.NotFound;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Services.Helpers;

public static class ProductHelper
{
    public static async Task<Farmer> ValidateUserAndFarmerAsync(
        string userId,
        bool trackChanges,
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            loggerManager.LogError($"[{nameof(ValidateUserAndFarmerAsync)}] User with id {userId} was not found!");
            throw new UserIdNotFoundException(Guid.Parse(userId));
        }

        var farmer = await repositoryManager
            .FarmerRepository
            .FindFarmersByConditionAsync(f => f.UserId.ToString() == userId, trackChanges)
            .FirstOrDefaultAsync();

        if (farmer is null)
        {
            loggerManager.LogError($"[{nameof(ValidateUserAndFarmerAsync)}] Farmer with id {farmer.Id} was not found!");
            throw new FarmerIdNotFoundException(Guid.Parse(userId));
        }

        return farmer;
    }

    public static async Task ValidateCategoryAsync(Guid categoryId,
        bool trackChanges,
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager)
    {
        if (!await repositoryManager.CategoryRepository.DoesCategoryExistByIdAsync(categoryId, trackChanges))
        {
            loggerManager.LogError($"[{nameof(ValidateCategoryAsync)}] Category creation failed: Category with Id '{categoryId}' not found.");
            throw new CategoryIdNotFoundException(categoryId);
        }
    }

    public static async Task UploadProductPhotosAsync(
        ICollection<IFormFile> photos,
        Product product,
        string uploadDirectory,
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager)
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
                await repositoryManager.ProductPhotoRepository.CreateProductPhotoAsync(photo);
            }

            await repositoryManager.SaveAsync();
            loggerManager.LogInfo($"[{nameof(UploadProductPhotosAsync)}] Successfully create photos for produc with Id {product.Id} at Date: {DateTime.UtcNow}.");
        }
    }

    public static async Task DeleteProductPhotoAsync(
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager,
        Guid productId,
        bool trackChanges)
    {
        var productPhotosForDeleting = await
            repositoryManager
            .ProductPhotoRepository
            .FindProductPhotoByConditionAsync(ph => ph.ProductId == productId, trackChanges)
            .ToListAsync();

        if(productPhotosForDeleting is null)
        {
            loggerManager.LogError($"[{nameof(DeleteProductPhotoAsync)}] Product with Id {productId} was not found at Date: {DateTime.UtcNow}");
            throw new ProductIdNotFoundException(productId); ;
        }

        try
        {
            foreach(var photo in productPhotosForDeleting)
            {
                 repositoryManager.ProductPhotoRepository.DeleteProductPhoto(photo);
            }

            await repositoryManager.SaveAsync();
            loggerManager.LogInfo($"Successfully delete photos of product with ID {productId}");
        }
        catch (Exception ex)
        {
            loggerManager.LogError($"An unexpected error occurred: {ex.Message}");
            throw new DeleteProductPhotosException(productId);
        }
    }
}
