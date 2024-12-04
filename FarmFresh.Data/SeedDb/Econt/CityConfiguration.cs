using FarmFresh.Data.Models;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Data.SeedDb.Econt
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey(c => c.CountryId);

            builder.OwnsOne(c => c.Location, navigation =>
            {
                navigation.Property(l => l.Latitude).HasColumnName("Latitude");
                navigation.Property(l => l.Longitude).HasColumnName("Longitude");
                navigation.Property(l => l.Confidence).HasColumnName("Confidence");
            });

            builder.OwnsMany(c => c.ServingOffices, navigation =>
            {
                navigation.ToTable("ServingOffices");
                navigation.HasKey("Id");
                navigation.WithOwner().HasForeignKey("CityId");
                navigation.Property(so => so.OfficeCode).HasColumnName("OfficeCode");
                navigation.Property(so => so.ServingType).HasColumnName("ServingType");
            });
        }
    }
}
