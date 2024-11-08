using Hospital.Domain.Entities.HealthProfiles;
using Hospital.Domain.Entities.HealthServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class HealthProfileEntityTypeConfiguration : IEntityTypeConfiguration<HealthProfile>
    {
        public void Configure(EntityTypeBuilder<HealthProfile> builder)
        {
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.CICode)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(15)");

            builder.Property(x => x.Phone)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(12)");

            builder.Property(x => x.Dob)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Pname)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Dname)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Wname)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Address)
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.Ethinic)
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.Created)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Modified)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Deleted)
                   .HasColumnType("DATETIME");

            builder.HasMany(x => x.Bookings)
                   .WithOne(x => x.HealthProfile)
                   .HasForeignKey(x => x.HealthProfileId);

        }
    }
}
