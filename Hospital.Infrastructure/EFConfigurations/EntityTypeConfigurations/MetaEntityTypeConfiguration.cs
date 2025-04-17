using Hospital.Domain.Entities.Metas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class MetaEntityTypeConfiguration : IEntityTypeConfiguration<Meta>
    {
        public void Configure(EntityTypeBuilder<Meta> builder)
        {
            builder.Property(x => x.Name)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Title)
                   .HasColumnType("NVARCHAR(1024)");

            builder.Property(x => x.Content)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(MAX)");

            builder.Property(x => x.Module)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Page)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.CreatedAt)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.ModifiedAt)
                   .HasColumnType("DATETIME");
        }
    }
}
