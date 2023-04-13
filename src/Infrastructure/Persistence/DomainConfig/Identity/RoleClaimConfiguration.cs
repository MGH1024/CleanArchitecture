using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.TableNameConfig;

namespace Persistence.DomainConfig.Public;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable(DatabaseTableName.RoleClaim, DatabaseSchema.SchemaSecurity);

        builder.Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(t => t.ClaimType)
            .HasMaxLength(512);

        builder.Property(t => t.ClaimValue)
             .HasMaxLength(256);

    }
}

