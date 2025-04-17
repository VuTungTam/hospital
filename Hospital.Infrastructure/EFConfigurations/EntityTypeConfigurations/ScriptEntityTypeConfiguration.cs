using Hospital.Domain.Entities.Metas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class ScriptEntityTypeConfiguration : IEntityTypeConfiguration<Script>
    {
        public void Configure(EntityTypeBuilder<Script> builder)
        {
            builder.Property(x => x.CreatedAt)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.ModifiedAt)
                   .HasColumnType("DATETIME");
        }
    }
}
