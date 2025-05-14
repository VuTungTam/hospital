using Hospital.Domain.Entities.CancelReasons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
       public class CancelReasonEntityTypeConfiguration : IEntityTypeConfiguration<CancelReason>
       {
              public void Configure(EntityTypeBuilder<CancelReason> builder)
              {
                     builder.Property(x => x.Reason)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(512)");

                     builder.HasOne(x => x.Booking)
                            .WithOne(x => x.CancelReason)
                            .HasForeignKey<CancelReason>(x => x.BookingId);
              }
       }
}
