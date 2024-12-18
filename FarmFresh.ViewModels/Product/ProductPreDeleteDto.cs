namespace FarmFresh.ViewModels.Product;

public record ProductPreDeleteDto
{
    public ProductPreDeleteDto()
    {
        Photos = new HashSet<ProductPhotosDto>();
    }

    public string Name { get; init; } = string.Empty;

    public string Origin { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public string PhotoString { get; init; }

    public string CategoryName { get; init; }

    public Guid Id { get; init; }

    public virtual IEnumerable<ProductPhotosDto> Photos { get; init; } = new HashSet<ProductPhotosDto>();
}
