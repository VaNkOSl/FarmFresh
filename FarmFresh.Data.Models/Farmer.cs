﻿using FarmFresh.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static FarmFresh.Commons.EntityValidationConstants.Farmers;

namespace FarmFresh.Data.Models;

public class Farmer : Entity_1<Guid>
{
    [Required]
    [MaxLength(FarmerDescriptionMaxLength)]
    public string FarmDescription { get; set; } = string.Empty;

    [Required]
    [MaxLength(FarmerLocationMaxLegth)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [MaxLength(FarmerPhoneNumberMaxLength)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public byte[] Photo { get; set; } = new byte[0];

    [Required]
    public string Egn {  get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }
    
    public Status FarmerStatus { get; set; }

    public Guid UserId { get; set; }

    public virtual ApplicationUser User { get; set; } = null!;

    public virtual ICollection<Product> OwnedProducts { get; set; }

    public Farmer()
    {
        Id = Guid.NewGuid();
        OwnedProducts = new HashSet<Product>();
    }

    public Farmer(
        Guid id,
        string description,
        string location,
        string phoneNumber,
        byte[] photo,
        string egn,
        DateTime dateOfBirth,
        Status farmerStatus,
        Guid userId)
    {
        Id = id;
        FarmDescription = description;
        Location = location;
        PhoneNumber = phoneNumber;
        Photo = photo;
        Egn = egn;
        DateOfBirth = dateOfBirth;
        FarmerStatus = farmerStatus;
        UserId = userId;
    }
}
                                                            