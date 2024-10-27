using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmFresh.Data.Models;

public class Review : Entity_1<Guid>
{
    public Review()
    {
        Id = Guid.NewGuid();
    }

    public string Content { get; set; } = string.Empty;

    public int Rating { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; } = null!;

    public DateTime ReviewDate { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;
}