using Config.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence.Repositories.Base;
using Serilog;
using System.Reflection;

var serilogConfig = new ConfigurationBuilder()
           .AddJsonFile("logsettings.json", optional: true, reloadOnChange: true)
           .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(serilogConfig)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.MainConfigureServices(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SecondApi",
        Version = "v1",
        Description = "Second Api"
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    var securitySchema = new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
            {
                    {securitySchema, new[] {"Bearer"}}
            };

    c.AddSecurityRequirement(securityRequirement);
});
var app = builder.Build();

try
{
    Log.Information("web starting up ...");
    await AppSeedData(app);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error in web");
}
finally
{
    app.MainConfigure(app.Environment);
    await app.RunAsync();
    Log.CloseAndFlush();
}

static async Task AppSeedData(IHost host)
{
    Log.Information("AppSeedData ...");
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    var seedService = services.GetRequiredService<ISeedService>();
    await seedService.SeedIdentityItemsAsync();
}
