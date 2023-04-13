using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.TableNameConfig;

namespace Persistence.DomainConfig.Public;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(DatabaseTableName.Role, DatabaseSchema.SchemaSecurity);

        builder.Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Description)
            .HasMaxLength(256);



        builder.Property(t => t.Name)
             .HasMaxLength(256)
             .IsRequired();

        builder.Property(t => t.NormalizedName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(t => t.ConcurrencyStamp)
            .HasMaxLength(1024)
            .IsRequired();
    }
}

