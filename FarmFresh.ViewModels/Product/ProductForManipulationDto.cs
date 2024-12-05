using FarmFresh.Data.Models.Enums;
using FarmFresh.ViewModels.Categories;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.ViewModels.Product;

public abstract record ProductForManipulationDto
{
    protected ProductForManipulationDto()
    {
        Categories = new HashSet<AllCategoriesDTO>();
        Photos = new HashSet<IFormFile>();
    }

    [Required]
    [Display(Name = "Product name.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Description of a product.")]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Origin of a product.")]
    public string Origin { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Product price.")]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Quantity of a product.")]
    public int StockQuantity { get; set; }

    public byte[] Photo { get; init; } = new byte[0];

    public string? PhotoString { get; init; }

    [DataType(DataType.Upload)]
    public IFormFile? PhotoFile { get; init; }

    [Required]
    [Display(Name = "Choose Suitable Season.")]
    public Seasons SuitableSeason { get; set; }

    [Required]
    [Display(Name = "Enter Harvest Date")]
    public DateTime HarvestDate { get; set; }

    [Required]
    [Display(Name = "Enter Expiration Date")]
    public DateTime ExpirationDate { get; set; }

    [Required]
    [Display(Name = "Category")]
    public Guid CategoryId { get; set; }

    public virtual IEnumerable<AllCategoriesDTO> Categories { get; set; }

    public virtual ICollection<IFormFile> Photos { get; set; }
}
