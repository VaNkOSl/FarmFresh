using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Enums;
using FarmFresh.ViewModels.Product;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.ViewModels.Order;

public class CreateOrderDto
{
    public CreateOrderDto()
    {
        Photos = new HashSet<ProductPhotosDto>();
    }

    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
    public string City { get; set; }
    public string EcontOfficeAddress { get; set; }

    public string Adress { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Num { get; set; } = string.Empty; 
    public string PhoneNumber { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public DateTime CreateOrderdDate { get; set; }

    public DeliveryOption DeliveryOption { get; set; }

    public OrderStatus? OrderStatus { get; set; }

    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; }

    public IEnumerable<ProductPhotosDto> Photos { get; set; }
}
