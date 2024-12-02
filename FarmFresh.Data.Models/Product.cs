using FarmFresh.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FarmFresh.Commons.EntityValidationConstants.Products;

namespace FarmFresh.Data.Models;

public class Product : Entity_1<Guid>
{
    public Product()
    {
        Id = Guid.NewGuid();

        Orders = new HashSet<Order>();
        Reviews = new HashSet<Review>();
        ProductPhotos = new HashSet<ProductPhoto>();
    }

    [Required]
    [MaxLength(ProductNameMaxLength)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(ProductDescriptionMaxLength)]
    public string? Description {  get; set; }

    [Required]
    [MaxLength(ProductOriginMaxLength)]
    public string Origin { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int StockQuantity { get; set; }

    public bool IsAvailable { get; set; }

    [Required]
    public byte[] Photo { get; set; } = new byte[0];

    [Required]
    public Seasons SuitableSeason {  get; set; }

    [Required]
    public DateTime HarvestDate { get; set; }

    [Required]
    public DateTime ExpirationDate { get; set; }

    public decimal? DiscountPrice { get; set; }

    [Required]
    public Guid FarmerId { get; set; }

    [ForeignKey(nameof(FarmerId))]
    public virtual Farmer Farmer { get; set; } = null!;

    [Required]
    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }

    public virtual ICollection<ProductPhoto> ProductPhotos { get; set; }
    public bool IsApproved { get; set; } // Defaults to false (not approved)

}
