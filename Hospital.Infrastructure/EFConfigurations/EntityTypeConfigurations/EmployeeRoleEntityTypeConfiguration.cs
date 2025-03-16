using Hospital.SharedKernel.Domain.Entities.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Persistences.EF.EntityTypeConfigurations
{
    public class EmployeeRoleEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeRole>
    {
        public void Configure(EntityTypeBuilder<EmployeeRole> builder)
        {
            builder.Property(x => x.CreatedAt)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.IsDeleted)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnType("bit");
        }
    }
}
