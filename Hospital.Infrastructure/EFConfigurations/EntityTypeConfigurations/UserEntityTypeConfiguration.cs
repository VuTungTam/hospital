using Hospital.SharedKernel.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.EFConfigurations.EntityTypeConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(32)");

            builder.Property(x => x.Username)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Password)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.PasswordHash)
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Salt)
                   .HasColumnType("NVARCHAR(8)");

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

            builder.Property(x => x.IsCustomer)
                    .IsRequired()
                    .HasDefaultValue(true)
                    .HasColumnType("BIT");

            builder.Property(x => x.LastPurchase)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.TotalSpending)
                   .HasColumnType("DECIMAL(19, 2)");

            builder.Property(x => x.Created)
                   .IsRequired()
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Modified)
                   .HasColumnType("DATETIME");

            builder.Property(x => x.Deleted)
                   .HasColumnType("DATETIME");

            builder.HasIndex(x => x.Code)
                   .IsUnique();

            builder.HasIndex(x => x.Email);

            builder.HasIndex(x => x.Username);

            builder.HasIndex(x => x.PasswordHash);

            builder.HasIndex(x => x.Phone);

            builder.HasIndex(x => x.Name);
        }
    }
}
