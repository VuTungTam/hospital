using Hospital.Domain.Entities.HeathServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class HealthServiceEntityTypeConfiguration : IEntityTypeConfiguration<HealthService>
    {
        public void Configure(EntityTypeBuilder<HealthService> builder)
        {
            builder.ToTable("HealthServices");
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Price)
                   .IsRequired()
                   .HasColumnType("DECIMAL(18,2)");

            builder.HasOne(x => x.ServiceType)
                   .WithMany(x => x.Services)
                   .HasForeignKey(x => x.TypeId);

            builder.HasOne(x => x.FacilitySpecialty)
                   .WithMany(x => x.Services)
                   .HasForeignKey(x => x.FacilitySpecialtyId);
        }
    }
}
