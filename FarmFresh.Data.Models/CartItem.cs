using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.Data.Models;

public class CartItem
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; } = null!;

    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    public int Quantity { get; set; }
}
