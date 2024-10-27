using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmFresh.Data.Models;

public class OrderProduct : Entity_1<Guid>
{
    public OrderProduct()
    {
        Id = Guid.NewGuid();
    }

    [Required]
    public Guid OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public virtual Order Order { get; set; } = null!;

    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;
    
    public int Quantity { get; set; }

    public decimal Price { get; set; }
}
