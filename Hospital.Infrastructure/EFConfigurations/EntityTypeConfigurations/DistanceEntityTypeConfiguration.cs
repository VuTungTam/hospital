using Hospital.Domain.Entities.Distances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class DistanceEntityTypeConfiguration : IEntityTypeConfiguration<Distance>
    {
        public void Configure(EntityTypeBuilder<Distance> builder)
        {
            builder.Property(x => x.SourceLatitude)
                   .HasColumnType("DOUBLE");

            builder.Property(x => x.SourceLongitude)
                   .HasColumnType("DOUBLE");

            builder.Property(x => x.DestinationLatitude)
                   .HasColumnType("DOUBLE");

            builder.Property(x => x.DestinationLatitude)
                   .HasColumnType("DOUBLE");

            builder.Property(x => x.DistanceMeter)
                   .HasColumnType("DOUBLE");

            builder.Property(x => x.Duration)
                   .HasColumnType("INT");

            builder.Property(x => x.CreatedAt)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("CURRENT_TIMESTAMP()");
        }
    }
}
