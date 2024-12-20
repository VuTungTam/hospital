﻿using Hospital.Domain.Entities.HealthServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class HealthServiceEntityTypeConfiguration : IEntityTypeConfiguration<HealthService>
    {
        public void Configure(EntityTypeBuilder<HealthService> builder)
        {
            builder.ToTable("HealthServices");
            builder.Property(x => x.NameVn)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.NameEn)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.DescriptionVn)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.DescriptionEn)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Price)
                   .IsRequired()
                   .HasColumnType("DECIMAL(18,2)");

            builder.Property(x => x.Created)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Modified)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Deleted)
                   .HasColumnType("DATETIME");

            builder.HasOne(x => x.ServiceType)
                   .WithMany(x => x.Services)
                   .HasForeignKey(x => x.TypeId);

            builder.HasOne(x => x.FacilitySpecialty)
                   .WithMany(x => x.Services)
                   .HasForeignKey(x => x.FacilitySpecialtyId);

            builder.HasMany(x => x.Bookings)
                   .WithOne(x => x.Service)
                   .HasForeignKey(x => x.ServiceId);
        }
    }
}
