using FarmFresh.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Data.SeedDb;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {

        builder
        .Property(p => p.Price)
        .HasPrecision(18, 2);

        builder
            .HasOne(o => o.Order)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(o => o.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
