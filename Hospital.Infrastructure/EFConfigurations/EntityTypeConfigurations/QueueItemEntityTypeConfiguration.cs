using Hospital.Domain.Entities.QueueItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class QueueItemEntityTypeConfiguration : IEntityTypeConfiguration<QueueItem>
    {
        public void Configure(EntityTypeBuilder<QueueItem> builder)
        {
            builder.ToTable("QueueItems");

            

            builder.Property(x => x.State)
                   .IsRequired()
                   .HasColumnType("int")
                   .HasDefaultValue(0);

            builder.Property(x => x.Position)
                   .IsRequired()
                   .HasColumnType("int");
            builder.Property(x => x.Created)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
