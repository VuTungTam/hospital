using Hospital.SharedKernel.Domain.Entities.Systems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class SystemConfigurationEntityTypeConfiguration : IEntityTypeConfiguration<SystemConfiguration>
    {
        public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
        {
            builder.Property(x => x.IsEnabledVerifiedAccount)
                    .IsRequired()
                    .HasDefaultValue(true)
                    .HasColumnType("BIT");

            builder.Property(x => x.BookingNotificationEmail)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.ModifiedAt)
                   .HasColumnType("DATETIME");
        }
    }
}
