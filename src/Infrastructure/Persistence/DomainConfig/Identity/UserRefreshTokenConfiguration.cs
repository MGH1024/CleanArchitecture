﻿using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.TableNameConfig;

namespace Persistence.DomainConfig.Identity;
public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.ToTable(DatabaseTableName.UserRefreshToken, DatabaseSchema.SchemaSecurity);

        builder.Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Token)
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(t => t.RefreshToken)
          .HasMaxLength(2048)
          .IsRequired();

        builder.Property(t => t.CreatedDate)
        .IsRequired();

        builder.Property(t => t.ExpirationDate)
        .IsRequired();

        builder.Property(t => t.IpAddress)
        .HasMaxLength(20);


        //navigations
        builder.HasOne<User>(a => a.User)
       .WithMany(a => a.UserRefreshTokens)
       .HasForeignKey(a => a.UserId)
       .OnDelete(DeleteBehavior.Restrict);
    }
}
