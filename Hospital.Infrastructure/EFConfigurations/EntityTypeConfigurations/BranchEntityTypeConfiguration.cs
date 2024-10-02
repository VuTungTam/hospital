using Hospital.SharedKernel.Domain.Entities.Branches;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class BranchEntityTypeConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Phone)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(50)");

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Address)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Active)
                   .IsRequired()
                   .HasColumnType("BIT")
                   .HasDefaultValue(true);


            builder.Property(x => x.FoundingDate)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Created)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Modified)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Deleted)
                   .HasColumnType("DATETIME");
        }
    }
}
