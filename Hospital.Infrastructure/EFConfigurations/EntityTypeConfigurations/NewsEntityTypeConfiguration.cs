using Hospital.Domain.Entities.Newses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class NewsEntityTypeConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.Property(x => x.Image)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.TitleSeo)
                   .HasColumnType("NVARCHAR(1024)");

            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(1024)");

            builder.Property(x => x.TitleEn)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(1024)");

            builder.Property(x => x.Slug)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(1048)");

            builder.Property(x => x.Content)
                   .IsRequired();

            builder.Property(x => x.ContentEn)
                   .IsRequired();

            builder.Property(x => x.PostDate)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Status)
                    .IsRequired()
                    .HasColumnType("SMALLINT");

            builder.Property(x => x.IsHighlight)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnType("BIT");

            builder.Property(x => x.Created)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Modified)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Deleted)
                   .HasColumnType("DATETIME");

            builder.HasIndex(x => x.Title);
        }
    }
}
