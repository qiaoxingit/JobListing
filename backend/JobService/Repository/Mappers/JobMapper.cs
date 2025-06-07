using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLib.Contracts.JobService;
using SharedLib.Database;

namespace JobService.Repository.Mappers;

/// <summary>
/// Configures the Job entity mapping to the JOB table
/// </summary>
public class JobMapper : IEntityTypeConfiguration<Job>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("JOB");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
               .HasColumnName("ID")
               .HasColumnType("binary(16)")
               .HasConversion(new MySqlGuidConverter())
               .IsRequired();

        builder.Property(u => u.Title)
               .HasColumnName("TITLE")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(u => u.Description)
               .HasColumnName("DESCRIPTION")
               .HasColumnType("text")
               .IsRequired();

        builder.Property(u => u.PostedDate)
               .HasColumnName("DATE_POSTED")
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(u => u.ExpireDate)
               .HasColumnName("DATE_EXPIRED")
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(u => u.PostedByUser)
               .HasColumnName("POSTED_BY_USER")
               .HasColumnType("binary(16)")
               .HasConversion(new MySqlGuidConverter())
               .IsRequired();

        builder.HasIndex(j => j.PostedByUser)
               .HasDatabaseName("POSTED_BY_USER");
    }
}
