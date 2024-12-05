namespace FarmFresh.ViewModels.Product;

public class ProductPhotosDto
{
    public Guid Id { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public Guid ProductId { get; set; }
}
