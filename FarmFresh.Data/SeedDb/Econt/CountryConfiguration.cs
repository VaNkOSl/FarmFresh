using FarmFresh.Data.Models.Econt.Nomenclatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static FarmFresh.Commons.EntityValidationConstants.Country;

namespace FarmFresh.Data.SeedDb.Econt
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code2)
                .IsRequired()
                .HasMaxLength(Code2MaxLength);
            builder.HasIndex(c => c.Code2)
                .IsUnique();
            
            builder.Property(c => c.Code3)
                .IsRequired()
                .HasMaxLength(Code3MaxLength);
            builder.HasIndex(c => c.Code3).IsUnique();
        }
    }
}
