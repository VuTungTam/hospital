using Hospital.Domain.Entities.HealthFacilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class HealthFacilityEntityTypeConfiguration : IEntityTypeConfiguration<HealthFacility>
    {
        public void Configure(EntityTypeBuilder<HealthFacility> builder)
        {
            builder.ToTable("HealthFacilities");
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
            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Phone)
                   .HasColumnType("NVARCHAR(50)");

            builder.Property(x => x.Website)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Dname)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Wname)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Address)
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.Latitude)
                   .IsRequired()
                   .HasColumnType("DECIMAL(9,6)");

            builder.Property(x => x.Longtitude)
                   .IsRequired()
                   .HasColumnType("DECIMAL(9,6)");

            builder.Property(x => x.Created)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Modified)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Deleted)
                   .HasColumnType("DATETIME");


            builder.HasOne(x => x.Category)
                   .WithMany(x => x.Facilities)
                   .HasForeignKey(x => x.CategoryId);

            builder.HasMany(x => x.FacilitySpecialties)
                   .WithOne(x => x.Facility)
                   .HasForeignKey(x => x.FacilityId);
        }
    }
}
