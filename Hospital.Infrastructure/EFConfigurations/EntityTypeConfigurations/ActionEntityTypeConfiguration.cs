using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class ActionEntityTypeConfiguration : IEntityTypeConfiguration<SharedKernel.Domain.Entities.Auths.Action>
    {
        public void Configure(EntityTypeBuilder<SharedKernel.Domain.Entities.Auths.Action> builder)
        {
            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(32)");

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.NameEn)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(1024)");

            builder.HasMany(x => x.RoleActions)
                   .WithOne(x => x.Action)
                   .HasForeignKey(x => x.ActionId);

            builder.HasIndex(x => x.Code);
        }
    }
}
