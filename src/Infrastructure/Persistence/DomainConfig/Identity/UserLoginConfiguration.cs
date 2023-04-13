
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.TableNameConfig;

namespace Persistence.DomainConfig.Public;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable(DatabaseTableName.UserLogin, DatabaseSchema.SchemaSecurity);


        builder.Property(t => t.ProviderDisplayName)
            .HasMaxLength(512);
    }
}

