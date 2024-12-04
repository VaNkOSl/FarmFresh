using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.SeedDb;
using FarmFresh.Data.SeedDb.Econt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FarmFresh.Data;

public class FarmFreshDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public FarmFreshDbContext(DbContextOptions<FarmFreshDbContext> options)
    : base(options)
    {
    }

    public FarmFreshDbContext()
    {
    }

    public virtual DbSet<Farmer> Farmers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPhoto> ProductPhotos { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<FarmerLocation> FarmerLocations { get; set; }

    //Econt DbSets
    //** SUBJECT TO CHANGE ** (if changed, don't forget the configurations as well)

    //public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    //public virtual DbSet<Office> Offices { get; set; }

    //public virtual DbSet<Quarter> Quarters { get; set; }

    //public virtual DbSet<Street> Streets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new OrderProductConfiguration());
        builder.ApplyConfiguration(new ReviewConfiguration());
        builder.ApplyConfiguration(new ProductPhotoConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());

        //builder.ApplyConfiguration(new AddressConfiguration());
        builder.ApplyConfiguration(new CityConfiguration());
        builder.ApplyConfiguration(new CountryConfiguration());
        //builder.ApplyConfiguration(new OfficeConfiguration());
        //builder.ApplyConfiguration(new QuarterConfiguration());
        //builder.ApplyConfiguration(new StreetConfiguration());
    }
}
