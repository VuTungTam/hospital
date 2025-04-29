using Hospital.Domain.Entities.HealthServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
       public class ServiceTypeEntityTypeConfiguration : IEntityTypeConfiguration<ServiceType>
       {
              public void Configure(EntityTypeBuilder<ServiceType> builder)
              {
                     builder.Property(x => x.NameVn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.NameEn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.Image)
                                 .IsRequired()
                                 .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.DescriptionVn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(MAX)");

                     builder.Property(x => x.DescriptionEn)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(MAX)");


              }
       }
}
