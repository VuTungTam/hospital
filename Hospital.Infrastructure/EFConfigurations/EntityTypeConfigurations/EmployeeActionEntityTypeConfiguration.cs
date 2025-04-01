using Hospital.SharedKernel.Domain.Entities.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class EmployeeActionEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeAction>
    {
        public void Configure(EntityTypeBuilder<EmployeeAction> builder)
        {
            builder.Property(x => x.IsExclude)
                   .IsRequired()
                   .HasColumnType("BIT")
                   .HasDefaultValueSql("0");

            builder.Property(x => x.CreatedAt)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
