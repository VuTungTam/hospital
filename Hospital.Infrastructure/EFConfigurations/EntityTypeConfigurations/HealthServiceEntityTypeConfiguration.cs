using Hospital.Domain.Entities.HealthServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class HealthServiceEntityTypeConfiguration : IEntityTypeConfiguration<HealthService>
    {
        public void Configure(EntityTypeBuilder<HealthService> builder)
        {
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

            builder.Property(x => x.CreatedAt)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.ModifiedAt)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.DeletedAt)
                   .HasColumnType("DATETIME");

            builder.HasOne(x => x.ServiceType)
                   .WithMany(x => x.Services)
                   .HasForeignKey(x => x.TypeId);

            builder.Property(x => x.TotalStars)
                   .HasColumnType("INT");

            builder.Property(x => x.TotalFeedback)
                   .HasColumnType("INT");

            builder.Property(x => x.StarPoint)
                   .HasColumnType("float");

            builder.HasMany(x => x.Bookings)
                   .WithOne(x => x.Service)
                   .HasForeignKey(x => x.ServiceId);
        }
    }
}
