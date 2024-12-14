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
    public class QuarterConfiguration : IEntityTypeConfiguration<Quarter>
    {
        public void Configure(EntityTypeBuilder<Quarter> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.Id)
                .ValueGeneratedNever();

            builder.HasOne<City>()
                .WithMany()
                .HasForeignKey(q => q.CityID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
