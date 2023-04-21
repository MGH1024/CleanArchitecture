using System.Text;
using System.Data;
using Persistence.Data;
using FluentValidation;
using Contract.Services;
using Config.Util.Filter;
using Castle.DynamicProxy;
using StackExchange.Redis;
using System.Globalization;
using System.Data.SqlClient;
using Config.Util.MiddleWares;
using Config.Util.Interceptors;
using Application.ConfigModels;
using Contract.Services.Public;
using Persistence.Repositories;
using Utility.AppSettingConfig;
using Utility.RouteConstraints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Contract.Validator.Public;
using Application.Services.Public;
using FluentValidation.AspNetCore;
using Domain.Contract.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Contract.Services.CachingProvider;
using Microsoft.AspNetCore.Localization;
using Contract.Services.DatetimeProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;
using Application.Services.CachingProvider;
using Contract.Services.DataShapingProvider;
using Application.Services.DateTimeProvider;
using Application.Services.DataShpingProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Config.Extensions;

public static class MainBootstrap
{
    private static readonly ProxyGenerator DynamicProxy = new();

    //startup ConfigureServices
    public static void MainConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        InitApiCoreConfig(services);
        InitCulture(services);
        InitConfigurations(services, config);
        InitDbContext(services, config);
        InitAuthentication(services, config);
        InitDapperContext(services, config);
        InitRepositories(services);
        InitServices(services, config);
        InitCors(services);
        InitFluentValidator(services);
        InitValidationDecorator(services);
    }

    private static void InitApiCoreConfig(IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });


        services.AddControllers(options => options.Filters.Add(typeof(ModelStateValidatorAttribute)));

        services.AddControllersWithViews()
            .AddNewtonsoftJson(op =>
                op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

        services.AddMvc(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
                setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            }).AddFluentValidation(options =>
            {
                options.ValidatorOptions.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
                options.DisableDataAnnotationsValidation = true;
                options.AutomaticValidationEnabled = true;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
    }

    private static void InitConfigurations(IServiceCollection services, IConfiguration config)
    {
        services.Configure<DbConnection>(option =>
            config.GetSection(nameof(DbConnection)).Bind(option));

        services.Configure<Redis>(option =>
            config.GetSection(nameof(Redis)).Bind(option));

        services.Configure<Cache>(option =>
            config.GetSection(nameof(Cache)).Bind(option));

        services.Configure<Auth>(option =>
            config.GetSection(nameof(Auth)).Bind(option));
    }

    private static void InitDbContext(IServiceCollection services, IConfiguration config)
    {
        //sqlserver
        var sqlConfig = config.GetSection(nameof(DbConnection)).Get<DbConnection>().SqlConnection;
        services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlConfig));

        //postgres
        //var postgresConfig = config.GetSection(nameof(DbConnection)).Get<DbConnection>().PostgresConnection;
        //services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
        //services.AddDbContext<AppDbContext>(options => options.UseNpgsql(DecryptionHelper.Decrypt(postgresConfig)));
    }

    private static void InitDapperContext(IServiceCollection services, IConfiguration config)
    {
        //sqlserver
        var sqlConfig = config.GetSection(nameof(DbConnection)).Get<DbConnection>().SqlConnection;
        services.AddTransient<IDbConnection>((sp) => new SqlConnection(sqlConfig));
    }

    private static void InitAuthentication(IServiceCollection services, IConfiguration config)
    {
        var auth = config.GetSection(nameof(Auth)).Get<Auth>();
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(auth.AuthKey);
                jwt.SaveToken = auth.SaveToken;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = auth.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = auth.ValidateIssuer,
                    ValidateAudience = auth.ValidateAudience,
                    RequireExpirationTime = auth.RequireExpirationTime,
                    ValidateLifetime = auth.ValidateLifetime,
                };
            });
    }

    private static void InitServices(IServiceCollection services, IConfiguration config)
    {
        //public
        services.AddTransient<IStateService, StateService>();
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient(typeof(IDataShaper<>), typeof(DataShaper<>));
        //services.AddTransient<>


        //core
        services.AddTransient<ISeedService, SeedService>();


        //caching
        var redisConnection = config.GetSection(nameof(Redis)).Get<Redis>().DefaultConnection;
        var configurationOptions = new ConfigurationOptions
        {
            ConnectRetry = redisConnection.ConnectRetry,
            AllowAdmin = redisConnection.AllowAdmin,
            AbortOnConnectFail = redisConnection.AbortOnConnectFail,
            DefaultDatabase = redisConnection.DefaultDatabase,
            ConnectTimeout = redisConnection.ConnectTimeout,
            Password = redisConnection.Password,
        };

        configurationOptions.EndPoints.Add(redisConnection.Host, redisConnection.Port);
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configurationOptions));

        services.AddTransient(typeof(ICachingService<>), typeof(CachingService<>));
    }

    private static void InitRepositories(IServiceCollection services)
    {
        //public
        services.AddTransient<IStateRep, StateRep>();
    }

    private static void InitCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }

    private static void InitCulture(IServiceCollection services)
    {
        var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

        services
            .Configure<RouteOptions>(routeOptions =>
            {
                routeOptions.ConstraintMap.Add(nameof(CultureRouteConstraint), typeof(CultureRouteConstraint));
            })
            .Configure<RequestLocalizationOptions>(requestLocalizationOptions =>
            {
                requestLocalizationOptions.DefaultRequestCulture =
                    new RequestCulture(CultureInfo.GetCultureInfo("en-US"));
                requestLocalizationOptions.SupportedCultures = supportedCultures;
                requestLocalizationOptions.SupportedUICultures = supportedCultures;
                requestLocalizationOptions.RequestCultureProviders.Insert(0, new CultureRequestCultureProvider());
            })
            .AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
    }

    private static void InitFluentValidator(IServiceCollection services)
    {
        services.AddFluentValidation(options =>
        {
            options.AutomaticValidationEnabled = false;
            options.RegisterValidatorsFromAssembly(typeof(GetStateByIdValidator).Assembly);
        });
    }

    private static void InitValidationDecorator(IServiceCollection services)
    {
        services.AddTransient<ValidationAsyncInterceptor>();
        var entityWithValidation = services
            .Where(x =>
                x.ServiceType.BaseType is { Namespace: { } } 
                &&
                x.ServiceType.BaseType.Namespace.Equals(typeof(AbstractValidator<>).Namespace))
            .Select(x =>
                x.ServiceType.BaseType.GenericTypeArguments.FirstOrDefault()).ToList();
        var serviceTypes = services.Where(x =>
            {
                var value = typeof(IBaseService).Namespace;
                return x.ServiceType.Namespace != null
                       &&
                       value != null 
                       &&
                       x.ServiceType.Namespace.StartsWith(value, StringComparison.Ordinal) 
                       //&&
                       //!x.ServiceType.Namespace.Equals(typeof(CommissionDtoValidation).Namespace, StringComparison.Ordinal)
                       &&
                       x.ServiceType.GetMethods()
                           .Select(x => x.GetParameters())
                           .SelectMany(x => x.ToList())
                           .Select(x => x.ParameterType)
                           .Intersect(entityWithValidation).Any();
            })
            .Select(x => x.ServiceType).ToList();

        DecorateServices(services, serviceTypes);
    }

    private static void DecorateServices(IServiceCollection services, List<Type> serviceTypes)
    {
        foreach (var serviceType in serviceTypes)
        {
            services.Decorate(serviceType,
                (target, serviceProvider) =>
                    DynamicProxy.CreateInterfaceProxyWithTargetInterface(
                        interfaceToProxy: serviceType,
                        target: target,
                        interceptors: serviceProvider.GetService<ValidationAsyncInterceptor>()
                    )
            );
        }
    }

    //startup Configure
    public static void MainConfigure(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        var requestLocalizationOptions =
            app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
        app.UseRequestLocalization(requestLocalizationOptions);

        app.UseCors(op => op.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
        }
        else
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("an unexpected fault happen.Try again Later.");
                });
            });
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}