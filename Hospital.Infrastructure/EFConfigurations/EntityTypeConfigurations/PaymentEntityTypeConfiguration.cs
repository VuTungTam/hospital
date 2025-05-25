using Hospital.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
       public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
       {
              public void Configure(EntityTypeBuilder<Payment> builder)
              {
                     builder.Property(x => x.TransactionContent)
                            .IsRequired()
                            .HasColumnType("NVARCHAR(512)");



                     builder.Property(x => x.Amount)
                            .IsRequired()
                            .HasColumnType("DECIMAL(18,2)");

                     builder.Property(x => x.CreatedAt)
                            .IsRequired()
                            .HasColumnType("DATETIME")
                            .HasDefaultValueSql("GETDATE()");

                     builder.HasOne(x => x.Booking)
                            .WithMany(x => x.Payments)
                            .HasForeignKey(x => x.BookingId);
              }
       }
}
