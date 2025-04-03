using Hospital.Domain.Entities.Doctors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class DoctorEntityTypeConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(32)");

            builder.Property(x => x.AliasLogin)
                   .HasColumnType("NVARCHAR(128)");

            builder.Property(x => x.Password)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.PasswordHash)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Phone)
                   .HasColumnType("NVARCHAR(50)");

            builder.Property(x => x.Name)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Dob)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Pname)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Dname)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Wname)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Address)
                   .HasColumnType("NVARCHAR(512)");

            builder.Property(x => x.ZaloId)
                    .HasColumnType("NVARCHAR(20)");

            builder.Property(x => x.Provider)
                    .HasColumnType("NVARCHAR(20)");

            builder.Property(x => x.PhotoUrl)
                    .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Json)
                    .HasColumnType("TEXT");

            builder.Property(x => x.Expertise)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Description)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.TrainingProcess)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.WorkExperience)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.LastSeen)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.IsDefaultPassword)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnType("bit");

            builder.Property(x => x.IsPasswordChangeRequired)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnType("bit");

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
                   .HasColumnType("bit");

            builder.HasIndex(x => x.Code)
                   .IsUnique();

            builder.HasIndex(x => x.Email);

            builder.HasIndex(x => x.Phone);

            builder.HasIndex(x => x.Name);

            builder.HasMany(x => x.HealthServices)
                   .WithOne(x => x.Doctor)
                   .HasForeignKey(x => x.DoctorId);
        }
    }
}
