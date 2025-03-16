using Hospital.Domain.Entities.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Persistences.EF.EntityTypeConfigurations
{
    public class ArticleEntityTypeConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
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

            builder.Property(x => x.Summary)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.SummaryEn)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.PostDate)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Status)
                    .IsRequired()
                    .HasColumnType("SMALLINT");

            builder.Property(x => x.IsHighlight)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnType("BIT");

            builder.Property(x => x.CreatedAt)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.ModifiedAt)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.DeletedAt)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.IsDeleted)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnType("BIT");

            builder.HasIndex(x => x.Title);
        }
    }
}
