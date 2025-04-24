using Hospital.Domain.Entities.Zones;
using Hospital.SharedKernel.Domain.Entities.Systems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
       public class ZoneEntityTypeConfiguration : IEntityTypeConfiguration<Zone>
       {
              public void Configure(EntityTypeBuilder<Zone> builder)
              {
                     builder.Property(x => x.NameVn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.NameEn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.LocationVn)
                          .IsRequired()
                          .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.LocationEn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.CreatedAt)
                           .IsRequired()
                           .HasColumnType("DATETIME")
                           .HasDefaultValueSql("GETDATE()");

                     builder.Property(x => x.ModifiedAt)
                            .HasColumnType("DATETIME");

                     builder.Property(x => x.DeletedAt)
                            .HasColumnType("DATETIME");

                     builder.HasMany(x => x.ZoneSpecialties)
                            .WithOne(x => x.Zone)
                            .HasForeignKey(x => x.ZoneId);
              }
       }
}
