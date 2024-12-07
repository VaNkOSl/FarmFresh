namespace LoggerService.Exceptions.NotFound;

public class ProductPhotoIdNotFoundException : NotFoundException
{
    public ProductPhotoIdNotFoundException(Guid photoId)
        : base($"Photo with ID '{photoId}' was not found. Please verify the database records.")
    {
    }
}
