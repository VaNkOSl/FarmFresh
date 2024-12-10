using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static FarmFresh.Commons.EntityValidationConstants.User;

namespace FarmFresh.Data.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        Id = Guid.NewGuid();

        Orders = new HashSet<Order>();
        Reviews = new HashSet<Review>();
    }

    [Required]
    [MaxLength(UserFirstNameMaxLength)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(UserLastNameMaxLength)]
    public string LastName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public bool IsBlocked { get; set; }

    public bool IsAdmin { get; set; }

    public virtual ICollection<Order> Orders { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }
}
