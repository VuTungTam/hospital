using Hospital.Domain.Entities.FacilityTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
       public class FacilityCategoryEntityTypeConfiguration : IEntityTypeConfiguration<FacilityType>
       {
              public void Configure(EntityTypeBuilder<FacilityType> builder)
              {
                     builder.Property(x => x.NameVn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.NameEn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.DescriptionVn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(MAX)");

                     builder.Property(x => x.DescriptionEn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(MAX)");

                     builder.Property(x => x.Slug)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(512)");
              }
       }
}
