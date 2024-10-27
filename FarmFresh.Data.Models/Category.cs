using System.ComponentModel.DataAnnotations;
using static FarmFresh.Commons.EntityValidationConstants.Category;

namespace FarmFresh.Data.Models;

public class Category : Entity_1<Guid>
{
    public Category()
    {
        Id = Guid.NewGuid();
        Products = new HashSet<Product>();
    }

    [Required]
    [MaxLength(CategoryNameMaxLength)]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Product> Products { get; set; }
}
