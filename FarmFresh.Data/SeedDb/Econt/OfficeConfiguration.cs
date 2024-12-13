using FarmFresh.Data.Models;
using FarmFresh.Data.Models.CustomComparers;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FarmFresh.Data.SeedDb.Econt
{
    public class OfficeConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                .ValueGeneratedNever();

            builder.Property(o => o.Phones)
                .HasConversion(
                    phones => JsonSerializer.Serialize(phones, (JsonSerializerOptions)null),
                    json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions)null))
                .HasColumnType("nvarchar(max)")
                .Metadata.SetValueComparer(ValueComparers.CollectionComparer<string>());

            builder.Property(o => o.Emails)
                .HasConversion(
                    emails => JsonSerializer.Serialize(emails, (JsonSerializerOptions)null),
                    json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions)null))
                .HasColumnType("nvarchar(max)")
                .Metadata.SetValueComparer(ValueComparers.CollectionComparer<string>());

            builder.Property(o => o.ShipmentTypes)
                .HasConversion(
                    shipmentTypes => JsonSerializer.Serialize(shipmentTypes, (JsonSerializerOptions)null),
                    json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions)null))
                .HasColumnType("nvarchar(max)")
                .Metadata.SetValueComparer(ValueComparers.CollectionComparer<string>());

            builder.HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
