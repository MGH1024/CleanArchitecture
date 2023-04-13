using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.TableNameConfig;

namespace Persistence.DomainConfig.Public;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable(DatabaseTableName.UseRole, DatabaseSchema.SchemaSecurity);
    }
}
