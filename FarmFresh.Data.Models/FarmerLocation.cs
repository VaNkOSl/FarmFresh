using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmFresh.Data.Models;

public class FarmerLocation : Entity_1<Guid>
{
    public FarmerLocation()
    {
        Id = Guid.NewGuid();
    }

    [Required]
    public double Longitude { get; set; }

    [Required]
    public double Latitude { get; set; }

    public string? Title { get; set; }

    [Required]
    public Guid FarmerId { get; set; }

    [ForeignKey(nameof(FarmerId))]
    public virtual Farmer Farmer { get; set; }

    public DateTime CreatedLocationDate { get; set; }
}
