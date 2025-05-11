using System.Security.Cryptography.X509Certificates;
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
                            .HasColumnType("DATE");

                     builder.Property(x => x.StartBooking)
                            .HasColumnType("TIME");

                     builder.Property(x => x.EndBooking)
                            .HasColumnType("TIME");
                     builder.Property(x => x.Phone)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(50)");
                     builder.Property(x => x.Email)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(255)");
                     builder.Property(x => x.CreatedAt)
                            .IsRequired()
                            .HasColumnType("DATETIME")
                            .HasDefaultValueSql("GETDATE()");

                     builder.Property(x => x.ModifiedAt)
                            .HasColumnType("DATETIME");

                     builder.Property(x => x.DeletedAt)
                            .HasColumnType("DATETIME");

                     builder.HasIndex(x => x.Code)
                            .IsUnique();

                     builder.HasOne(x => x.HealthFacility)
                            .WithMany(x => x.Bookings)
                            .HasForeignKey(x => x.FacilityId)
                            .OnDelete(DeleteBehavior.Restrict); ;
              }
       }
}
