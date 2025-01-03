using FarmFresh.Data.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static FarmFresh.Commons.EntityValidationConstants;
using static FarmFresh.Commons.EntityValidationConstants.User;

namespace FarmFresh.Data.Models;

public class ApplicationUser : IdentityUser<Guid>
{
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

    public ApplicationUser()
    {
        Id = Guid.NewGuid();

        Orders = new HashSet<Order>();
        Reviews = new HashSet<Review>();
    }

    public ApplicationUser(Guid id,
        string userName,
        string email,
        string firstName,
        string lastName,
        DateTime createdAt)
    {
        Id = id; 
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Email = email;
        CreatedAt = createdAt;
    }

    public override bool Equals(object? obj) => (obj as ApplicationUser)?.Id.Equals(Id) ?? false;
    public override int GetHashCode() => Id.GetHashCode();
}
