using Hospital.Domain.Entities.Symptoms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class SymptomEntityTypeConfiguration : IEntityTypeConfiguration<Symptom>
    {
        public void Configure(EntityTypeBuilder<Symptom> builder)
        {
            builder.ToTable("Symptoms");

            builder.Property(x => x.NameVn)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.NameEn)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.Created)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Modified)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Deleted)
                   .HasColumnType("DATETIME");

            //builder.HasMany(x => x.BookingSymptom)
            //       .WithOne(x => x.Symptom)
            //       .HasForeignKey(x => x.SymptomId);

        }
    }
}
