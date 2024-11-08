using Hospital.Domain.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(32)");

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasColumnType("SMALLINT");

            builder.Property(x => x.Date)
                   .IsRequired()
                   .HasColumnType("DATETIME");


            builder.Property(x => x.Created)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Modified)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Deleted)
                   .HasColumnType("DATETIME");

            builder.HasIndex(x => x.Code)
                   .IsUnique();

        }
    }
}
