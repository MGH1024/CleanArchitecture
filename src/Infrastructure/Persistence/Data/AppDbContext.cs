using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Domain.Entities.Public;
using Utility.AppSettingConfig;
using Persistence.DomainConfig.Public;
using Contract.Services.DatetimeProvider;
using Persistence.Extensions;
using Microsoft.AspNetCore.Identity;
using Utility.Decryption;

namespace Persistence.Data;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContext;
    private readonly string _connectionstring;
    private readonly IDateTime _dateTime;

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContext,
        IOptions<DbConnection> dbOption, IDateTime dateTime) : base(options)
    {
        _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));


        //sqlserver
        _connectionstring = DecryptionHelper.Decrypt(dbOption.Value.SqlConnection);


        //postgres
        //_connectionstring = DecryptionHelper.Decrypt(dbOption.Value.PostgresConnection);

    }

    public string CurrentUsername
    {
        get
        {
            if (_httpContext.HttpContext != null)
            {
                var name = _httpContext.HttpContext
                    .User
                    .Claims
                    .FirstOrDefault(x => x.Type.Equals("username", StringComparison.InvariantCultureIgnoreCase));

                if (name == null)
                    return "";
                return name.Value;
            }
            return "";
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var modifiedEntries = ChangeTracker.Entries()
            .Where(p => p.State == EntityState.Modified || p.State == EntityState.Added ||
                        p.State == EntityState.Deleted);
        foreach (var item in modifiedEntries)
        {
            var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
            if (entityType != null)
            {
                var createDate = entityType.FindProperty("CreatedDate");
                var updateDate = entityType.FindProperty("UpdatedDate");
                var deleteDate = entityType.FindProperty("DeletedDate");
                var createBy = entityType.FindProperty("CreatedBy");
                var updateBy = entityType.FindProperty("UpdatedBy");
                var deleteBy = entityType.FindProperty("DeletedBy");
                var isDeleted = entityType.FindProperty("IsDeleted");


                if (item.State == EntityState.Added && createDate != null && createBy != null)
                {
                    item.Property("CreatedDate").CurrentValue = _dateTime.IranNow;
                    item.Property("CreatedBy").CurrentValue = CurrentUsername;
                    item.Property("IsActive").CurrentValue = true;
                    item.Property("IsUpdated").CurrentValue = false;
                    item.Property("IsDeleted").CurrentValue = false;
                }

                if (item.State == EntityState.Modified && updateDate != null && updateBy != null)
                {
                    item.Property("UpdatedDate").CurrentValue = _dateTime.IranNow;
                    item.Property("UpdatedBy").CurrentValue = CurrentUsername;
                    item.Property("IsUpdated").CurrentValue = true;
                }

                if (item.State == EntityState.Deleted && deleteDate != null && deleteBy != null && isDeleted != null)
                {
                    item.Property("DeletedDate").CurrentValue = _dateTime.IranNow;
                    item.Property("DeletedBy").CurrentValue = CurrentUsername;
                    item.Property("IsDeleted").CurrentValue = true;
                    item.Property("IsActive").CurrentValue = false;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //public config
        builder.ApplyConfiguration(new StateConfiguration());

        //seed data
        builder.Seed();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //sql
        optionsBuilder
            .UseSqlServer(_connectionstring)
            .EnableSensitiveDataLogging();


        //postgres
        //optionsBuilder
        //    .UseNpgsql(_connectionstring)
        //    .EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }


    //public
    public DbSet<State> States { get; set; }
}