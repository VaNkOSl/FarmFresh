using FarmFresh.Data.Models.Enums;
using FarmFresh.ViewModels.Farmer;
using FarmFresh.ViewModels.Review;

namespace FarmFresh.ViewModels.Product;

public record ProductDetailsDto
{
    public ProductDetailsDto()
    {
        Photos = new HashSet<ProductPhotosDto>();
    }

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string? Description { get; set; }

    public string Origin { get; set; } = string.Empty;

    public Seasons SuitableSeason { get; set; }

    public DateTime HarvestDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string CategoryName { get; set; }

    public virtual FarmerProfileViewModel Farmer {  get; set; }

    public IEnumerable<ProductPhotosDto> Photos { get; set; }

    public List<ProductReviewDto> Reviews { get; set; }
}
