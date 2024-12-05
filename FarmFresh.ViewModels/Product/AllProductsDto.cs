namespace FarmFresh.ViewModels.Product;

public record AllProductsDto
{
    public AllProductsDto()
    {
        Photos = new HashSet<ProductPhotosDto>();
    }

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public IEnumerable<ProductPhotosDto> Photos { get; set; }
}
