using FarmFresh.Data.Models;
using FarmFresh.Data.SeedDb;
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new OrderProductConfiguration());
        builder.ApplyConfiguration(new ReviewConfiguration());
        builder.ApplyConfiguration(new ProductPhotoConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());
    }
}
