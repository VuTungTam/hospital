using Hospital.Domain.Entities.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class FeedbackEntityTypeConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {

            builder.Property(x => x.Message)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(MAX)");

            builder.Property(x => x.Stars)
                   .HasColumnType("INT");

            builder.Property(x => x.CreatedAt)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.ModifiedAt)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.BookingCode)
                   .IsRequired()
                   .HasColumnType("VARCHAR(32)");

            builder.HasOne(f => f.Booking)
                   .WithOne(b => b.Feedback)
                   .HasForeignKey<Feedback>(f => f.BookingId) 
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
