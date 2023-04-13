using Persistence.Repositories.Base;
using Serilog;

namespace Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var serilogConfig = new ConfigurationBuilder()
            .AddJsonFile("logsettings.json", optional: true, reloadOnChange: true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(serilogConfig)
            .CreateLogger();

        var host = CreateHostBuilder(args).Build();
        try
        {
            Log.Information("web starting up ...");
            await AppSeedData(host);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Error in web");
        }
        finally
        {
            await host.RunAsync();
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }

    public static async Task AppSeedData(IHost host)
    {
        Log.Information("AppSeedData ...");
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var seedService = services.GetRequiredService<ISeedService>();
        await seedService.SeedIdentityItemsAsync();
    }
}