using Hospital.SharedKernel.Modules.Notifications.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(x => x.Type)
                   .IsRequired()
                   .HasColumnType("TINYINT");

            builder.Property(x => x.IsUnread)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnType("BIT");

            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(MAX)");

            builder.Property(x => x.CreatedAt)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.ModifiedAt)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.DeletedAt)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.IsDeleted)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnType("BIT");
        }
    }
}
