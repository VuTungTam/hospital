using Hospital.Domain.Entities.Specialties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class SpecialtyEntityTypeConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.Property(x => x.NameVn)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.NameEn)
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

            builder.HasMany(x => x.FacilitySpecialties)
                   .WithOne(x => x.Specialty)
                   .HasForeignKey(x => x.SpecialtyId);

            builder.HasMany(x => x.HealthServices)
                   .WithOne(x => x.Specialty)
                   .HasForeignKey(x => x.SpecialtyId);
        }
    }
}
