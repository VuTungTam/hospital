using Hospital.Domain.Entities.ServiceTimeRules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
       public class ServiceTimeRuleEntityTypeConfiguration : IEntityTypeConfiguration<ServiceTimeRule>
       {
              public void Configure(EntityTypeBuilder<ServiceTimeRule> builder)
              {
                     builder.Property(x => x.StartTime)
                            .IsRequired()
                            .HasColumnType("Time");

                     builder.Property(x => x.StartBreakTime)
                            .IsRequired()
                            .HasColumnType("Time");

                     builder.Property(x => x.EndBreakTime)
                            .IsRequired()
                            .HasColumnType("Time");

                     builder.Property(x => x.EndTime)
                            .IsRequired()
                            .HasColumnType("Time");

                     builder.Property(x => x.CreatedAt)
                            .IsRequired()
                            .HasColumnType("DATETIME")
                            .HasDefaultValueSql("GETDATE()");

                     builder.Property(x => x.ModifiedAt)
                            .HasColumnType("DATETIME");

                     builder.Property(x => x.DeletedAt)
                            .HasColumnType("DATETIME");

                     builder.HasOne(x => x.Service)
                             .WithMany(x => x.ServiceTimeRules)
                             .HasForeignKey(x => x.ServiceId);

                     builder.Property(x => x.AllowWalkin)
                            .IsRequired()
                            .HasDefaultValue(false)
                            .HasColumnType("BIT");
              }
       }
}
