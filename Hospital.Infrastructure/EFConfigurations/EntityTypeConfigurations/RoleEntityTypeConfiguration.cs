using Hospital.SharedKernel.Domain.Entities.Auths;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(32)");

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.NameEn)
                   .HasColumnType("NVARCHAR(255)");

            builder.HasMany(x => x.RoleActions)
                   .WithOne(x => x.Role)
                   .HasForeignKey(x => x.RoleId);

            builder.HasIndex(x => x.Code);
        }
    }
}
