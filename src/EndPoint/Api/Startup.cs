using Application.Services.ExternalServices;
using Config.Extensions;
using Contract.Services.ExternalServices;
using Microsoft.OpenApi.Models;
using Polly.Extensions.Http;
using Polly;
using System.Reflection;
using Config.Util.MiddleWares;
using Mgh.Swagger.Extensions;

namespace Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.MainConfigureServices(_configuration);

        //swagger
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        services.AddSwagger(cfg =>
        {
            cfg.XmlComments = xmlPath;
            cfg.OpenApiInfo = new OpenApiInfo
            {
                Title = "clean Architecture",
                Version = "v1",
                Description = "clean architecture"
            };

            var openApiSecurityScheme = new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
            };

            cfg.OpenApiSecurityScheme = openApiSecurityScheme;
            cfg.OpenApiReference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            };
            cfg.OpenApiSecurityRequirement = new OpenApiSecurityRequirement
            {
                { openApiSecurityScheme, new[] { "Bearer" } }
            };
        });

        services.AddAutoMapper(typeof(Startup));

        //call httpclient by polly
        services.AddHttpClient<ISecondApiService, SecondApiService>(
                client =>
                {
                    client.BaseAddress = new Uri(_configuration["SecondApi:BaseUrl"]);
                    client.Timeout = TimeSpan.FromSeconds(30);
                })
            .AddPolicyHandler((provider, _) => GetRetryPolicy());


        //call httpclient by HttpClientFactory and custom handler
        services.AddHttpClient("SecondApi",
            httpClient => { httpClient.BaseAddress = new Uri(_configuration["SecondApi:BaseUrl"]); });
        services.AddTransient<ValidateHeaderHandler>();
        services.AddHttpClient("SecondApi")
            .AddHttpMessageHandler<ValidateHeaderHandler>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //app.UseMiddleware<SetTokenMiddleWare>();
        app.MainConfigure(env);
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    , onRetry: (_, __) =>
                    {
                        //log
                    })
            ;
    }
}