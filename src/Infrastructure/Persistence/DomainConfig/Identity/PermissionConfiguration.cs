using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.TableNameConfig;

namespace Persistence.DomainConfig.Public;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(DatabaseTableName.Permission, DatabaseSchema.SchemaSecurity);
        builder.Property(t => t.PermissionId)
           .IsRequired()
           .ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(t => t.Url)
          .HasMaxLength(256)
          .IsRequired();

        builder.Property(t => t.Description)
          .HasMaxLength(512);
    }
}

