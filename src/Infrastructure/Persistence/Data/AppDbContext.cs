using Domain.Entities.Public;
using Persistence.Extensions;
using Utility.AppSettingConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Persistence.DomainConfig.Public;
using Contract.Services.DatetimeProvider;

namespace Persistence.Data; 

public class AppDbContext : DbContext
{
    private readonly IDateTime _dateTime;
    private readonly string _connectionString;
    private readonly IHttpContextAccessor _httpContext;

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContext,
        IOptions<DbConnection> dbOption, IDateTime dateTime) : base(options)
    {
        _dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
        _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));


        //sqlserver
        _connectionString = dbOption.Value.SqlConnection;


        //postgres
        //_connectionString = DecryptionHelper.Decrypt(dbOption.Value.PostgresConnection);

    }

    private string CurrentUsername
    {
        get
        {
            if (_httpContext.HttpContext == null) return "";
            var name = _httpContext.HttpContext
                .User
                .Claims
                .FirstOrDefault(x => x.Type.Equals("username", StringComparison.InvariantCultureIgnoreCase));

            return name == null ? "" : name.Value;
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
            if (entityType == null) continue;
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

            if (item.State != EntityState.Deleted || deleteDate == null || deleteBy == null ||
                isDeleted == null) continue;
            item.Property("DeletedDate").CurrentValue = _dateTime.IranNow;
            item.Property("DeletedBy").CurrentValue = CurrentUsername;
            item.Property("IsDeleted").CurrentValue = true;
            item.Property("IsActive").CurrentValue = false;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(StateConfiguration).Assembly);

        //seed data
        builder.Seed();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //sql
        optionsBuilder
            .UseSqlServer(_connectionString)
            .EnableSensitiveDataLogging();


        //postgres
        //optionsBuilder
        //    .UseNpgsql(_connectionString)
        //    .EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }


    //public
    public DbSet<State> States { get; set; }
}