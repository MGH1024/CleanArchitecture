using Domain.Entities.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.TableNameConfig;

namespace Persistence.DomainConfig.Public;

public class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        //table setting section
        builder.ToTable(DatabaseTableName.State, DatabaseSchema.SchemaGeneral);


        //fix fileds section
        builder.Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();


        builder.Property(t => t.Title)
            .HasMaxLength(maxLength: 512)
            .IsRequired();

        builder.Property(t => t.Description)
           .HasMaxLength(maxLength: 256);



        //not mapped section
        builder.Ignore(a => a.Row);
        builder.Ignore(a => a.PageSize);
        builder.Ignore(a => a.TotalCount);
        builder.Ignore(a => a.CodeEnum);
        builder.Ignore(a => a.CurrentPage);
        builder.Ignore(a => a.ListItemText);
        builder.Ignore(a => a.ListItemTextForAdmins);

        //default value  section
        builder.Property(t => t.IsActive).HasDefaultValue(true);
        builder.Property(t => t.IsDeleted).HasDefaultValue(false);
        builder.Property(t => t.IsUpdated).HasDefaultValue(false);
    }
}
