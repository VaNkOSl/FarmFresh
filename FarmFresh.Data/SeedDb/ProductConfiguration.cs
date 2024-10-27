using FarmFresh.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Data.SeedDb;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .Property(p => p.Price)
            .HasPrecision(18, 2);
        
        builder
            .Property(p => p.DiscountPrice)
            .HasPrecision (18, 2);

        builder
            .HasOne(f => f.Farmer)
            .WithMany(f => f.OwnedProducts)
            .HasForeignKey(f => f.FarmerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(c => c.Category)
            .WithMany(p => p.Products)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(r => r.Reviews)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(ph => ph.ProductPhotos)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
