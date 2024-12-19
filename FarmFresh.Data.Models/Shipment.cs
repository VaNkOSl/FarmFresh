using System.ComponentModel.DataAnnotations;

namespace FarmFresh.Data.Models
{
    public class Shipment : Entity_1<Guid>
    {
        public Shipment()
        {
            Id = Guid.NewGuid();
        }

        [Required]
        public string ShipmentNumber { get; set; } = string.Empty;

        [Required]
        public Guid OrderId { get; set; }
    }
}
