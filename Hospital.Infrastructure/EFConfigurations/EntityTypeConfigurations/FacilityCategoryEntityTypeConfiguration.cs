using Hospital.Domain.Entities.HealthFacilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class FacilityCategoryEntityTypeConfiguration : IEntityTypeConfiguration<FacilityCategory>
    {
        public void Configure(EntityTypeBuilder<FacilityCategory> builder)
        {
            builder.ToTable("FacilityCategories");
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
            builder.Property(x => x.Created)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Deleted)
                   .HasColumnType("DATETIME");

        }
    }
}
