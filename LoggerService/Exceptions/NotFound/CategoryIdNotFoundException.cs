namespace LoggerService.Exceptions.NotFound;

public class CategoryIdNotFoundException : NotFoundException
{
    public CategoryIdNotFoundException(Guid categoryId)
        : base($"Category with ID '{categoryId}' was not found. Please verify the database records.")
    {
    }
}
