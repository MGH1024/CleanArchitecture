using Config.Extensions;
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
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
       
        app.MainConfigure(env);
    }
}