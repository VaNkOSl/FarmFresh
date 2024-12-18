using FarmFresh.Data.Models.Econt.Nomenclatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Data.SeedDb.Econt
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.City)
                .WithMany()
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsOne(a => a.Location, navigation =>
            {
                navigation.Property(l => l.Latitude).HasColumnName("Latitude");
                navigation.Property(l => l.Longitude).HasColumnName("Longitude");
                navigation.Property(l => l.Confidence).HasColumnName("Confidence");
            });
        }
    }
}
