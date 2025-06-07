using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLib.Contracts.UserService;
using SharedLib.Database;

namespace UserService.Repository.Mappers;

/// <summary>
/// Configures the User entity mapping to the USER table
/// </summary>
public class UserMapper : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("USER");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
               .HasColumnName("ID")
               .HasColumnType("binary(16)")
               .HasConversion(new MySqlGuidConverter())
               .IsRequired();

        builder.Property(u => u.FirstName)
               .HasColumnName("FIRST_NAME")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(u => u.LastName)
               .HasColumnName("LAST_NAME")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(u => u.Email)
               .HasColumnName("EMAIL")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(u => u.Username)
               .HasColumnName("USERNAME")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(u => u.Password)
               .HasColumnName("PASSWORD")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(u => u.Role)
               .HasColumnName("ROLE")
               .HasConversion<int>()
               .IsRequired();
    }
}
