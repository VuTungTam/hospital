using Hospital.Domain.Entities.HealthFacilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
       public class HealthFacilityEntityTypeConfiguration : IEntityTypeConfiguration<HealthFacility>
       {
              public void Configure(EntityTypeBuilder<HealthFacility> builder)
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

                     builder.Property(x => x.SummaryEn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(MAX)");

                     builder.Property(x => x.SummaryVn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(MAX)");

                     builder.Property(x => x.DescriptionEn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(MAX)");

                     builder.Property(x => x.Logo)
                            .HasColumnType("NVARCHAR(255)");

                     builder.Property(x => x.Dname)
                            .HasColumnType("NVARCHAR(255)");

                     builder.Property(x => x.Wname)
                            .HasColumnType("NVARCHAR(255)");

                     builder.Property(x => x.Address)
                            .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.MapURL)
                            .HasColumnType("NVARCHAR(MAX)");

                     builder.Property(x => x.TotalStars)
                            .HasColumnType("INT");

                     builder.Property(x => x.TotalFeedback)
                            .HasColumnType("INT");

                     builder.Property(x => x.StarPoint)
                            .HasColumnType("float");

                     builder.Property(x => x.CreatedAt)
                            .IsRequired()
                            .HasColumnType("DATETIME")
                            .HasDefaultValueSql("GETDATE()");

                     builder.Property(x => x.ModifiedAt)
                            .HasColumnType("DATETIME");

                     builder.Property(x => x.DeletedAt)
                            .HasColumnType("DATETIME");

                     builder.HasMany(x => x.HealthServices)
                            .WithOne(x => x.HealthFacility)
                            .HasForeignKey(x => x.FacilityId);

                     builder.HasMany(x => x.Zones)
                            .WithOne(x => x.HealthFacility)
                            .HasForeignKey(x => x.FacilityId);

                     builder.HasMany(x => x.Images)
                            .WithOne(x => x.HealthFacility)
                            .HasForeignKey(x => x.FacilityId);

                     builder.Property(x => x.Slug)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(512)");
              }
       }
}
