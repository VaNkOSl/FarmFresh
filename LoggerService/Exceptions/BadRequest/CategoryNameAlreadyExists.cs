namespace LoggerService.Exceptions.BadRequest;

public class CategoryNameAlreadyExists : BadRequestException
{
    public CategoryNameAlreadyExists() 
        : base("Category with provided name already exists!")
    {
    }
}
