using FarmFresh.Data.Models;
using Microsoft.AspNetCore.Http;

namespace FarmFresh.Services.Contacts;

public interface IProductPhotoService
{
    Task UploadProductPhotosAsync(ICollection<IFormFile> photos, string uploadDirectory, Product product);

    Task DeleteProductPhotoAsync(Guid photoId, bool trackChanges);
}
