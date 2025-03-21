using Hospital.Domain.Entities.Symptoms;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Domain.Entities.TimeSlots;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class TimeSlotEntityTypeConfiguration : IEntityTypeConfiguration<TimeSlot>
    {
        public void Configure(EntityTypeBuilder<TimeSlot> builder)
        {
            builder.Property(x => x.Start)
            .IsRequired()
            .HasColumnType("TIME");

            builder.Property(x => x.End)
                .IsRequired()
                .HasColumnType("TIME");

            builder.Property(x => x.TimeRuleId)
                .IsRequired();

            builder.HasOne(x => x.ServiceTimeRule)
                .WithMany(x => x.TimeSlots)
                .HasForeignKey(x => x.TimeRuleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.CreatedAt)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.DeletedAt)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.IsDeleted)
                  .IsRequired()
                  .HasDefaultValue(false)
                  .HasColumnType("BIT");
        }
    }
}
