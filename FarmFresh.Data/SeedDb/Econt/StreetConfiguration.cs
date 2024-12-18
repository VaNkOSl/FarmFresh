using FarmFresh.Data.Models.Econt.Nomenclatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmFresh.Data.SeedDb.Econt
{
    public class StreetConfiguration : IEntityTypeConfiguration<Street>
    {
        public void Configure(EntityTypeBuilder<Street> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .ValueGeneratedNever();

            builder.HasOne<City>()
                .WithMany()
                .HasForeignKey(s => s.CityID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
