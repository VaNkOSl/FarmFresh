using FarmFresh.Data.Models.Enums;
using FarmFresh.ViewModels.Product;

namespace FarmFresh.ViewModels.Admin;

public abstract record AdminProductreject
{
    public AdminProductreject()
    {
        Photos = new HashSet<ProductPhotosDto>();
    }

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string Origin { get; set; } = string.Empty;

    public Seasons SuitableSeason { get; set; }

    public DateTime HarvestDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string FarmerPhoneNumber { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public byte[] FarmerPhoto { get; set; }

    public string PhotoString { get; set; }

    public IEnumerable<ProductPhotosDto> Photos { get; set; }

    public string FarmerName { get; set; }

    public string FarmerEmail { get; set; }
}
