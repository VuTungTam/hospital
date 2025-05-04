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

                     builder.Property(x => x.PaymentUrl)
                             .IsRequired()
                             .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.ExternalTransactionId)
                             .IsRequired()
                             .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.BankBin)
                             .IsRequired()
                             .HasColumnType("NVARCHAR(20)");

                     builder.Property(x => x.Note)
                             .HasColumnType("NVARCHAR(512)");

                     builder.Property(x => x.Amount)
                            .IsRequired()
                            .HasColumnType("DECIMAL(18,2)");

                     builder.Property(x => x.CreatedAt)
                            .IsRequired()
                            .HasColumnType("DATETIME")
                            .HasDefaultValueSql("GETDATE()");

                     builder.Property(x => x.ExpiredAt)
                            .HasColumnType("DATETIME");

                     builder.Property(x => x.DeletedAt)
                            .HasColumnType("DATETIME");

                     builder.Property(x => x.IsDeleted)
                            .IsRequired()
                            .HasColumnType("BIT")
                            .HasDefaultValue(false);
                     builder.HasOne(x => x.Booking)
                            .WithMany(x => x.Payments)
                            .HasForeignKey(x => x.BookingId);
              }
       }
}
