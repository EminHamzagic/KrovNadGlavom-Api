using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using krov_nad_glavom_api.Application;
using krov_nad_glavom_api.Application.Interfaces;
using krov_nad_glavom_api.Application.Services;
using krov_nad_glavom_api.Application.Services.Interfaces;
using krov_nad_glavom_api.Application.Validators;
using krov_nad_glavom_api.Commands;
using krov_nad_glavom_api.Data;
using krov_nad_glavom_api.Data.Config;
using krov_nad_glavom_api.Infrastructure.MongoDB;
using krov_nad_glavom_api.Infrastructure.MySql;
using krov_nad_glavom_api.Infrastructure.Neo4j;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Neo4j.Driver;
using Serilog;

public static class Program
{
    public static string ALLOWED_SPECIFIC_ORIGINS_KEY = "_myAllowSpecificOrigins";

    public static async Task<int> Main(string[] args)
    {
        var command = args.FirstOrDefault(arg => !arg.StartsWith("--"));

        var app = CreateApp(args);
        app.UseCors(ALLOWED_SPECIFIC_ORIGINS_KEY);
        using var scope = app.Services.CreateScope();

        var allCommands = scope.ServiceProvider.GetServices<ICommand>().ToList();
        allCommands.Add(new ServeCommand(app, ALLOWED_SPECIFIC_ORIGINS_KEY));

        var matchingCommands = allCommands.Where(c => c.MatchCommand(command));
        var commandNames = allCommands.Select(c => c.GetCommandName()).ToList();

        if (string.IsNullOrEmpty(command)) return ExitAndLogNoMatchingCommands(commandNames);
        if (!matchingCommands.Any()) return ExitAndLogNoMatchingCommands(commandNames);

        foreach (var commandClass in matchingCommands)
        {
            await commandClass.InvokeAsync();
        }
        return 0;
    }

    private static WebApplication CreateApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var globalConfig = new GlobalConfig(builder.Configuration);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.Services.AddControllers()
            .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        builder.Services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", opt =>
            {
                opt.Window = TimeSpan.FromMinutes(1);
                opt.PermitLimit = 20;
                opt.QueueLimit = 0;
            });
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        ConfigureSwagger(builder.Services);
        ConfigureDbContext(builder.Services, globalConfig, args);
        ConfigureAuthentication(builder.Services, globalConfig.JWTSettings);
        ConfigureDependencies(builder.Services, globalConfig);
        ConfigureLogging(builder, globalConfig);
        ConfigureAutoMapper(builder.Services);
        ConfigureFluentValidataion(builder.Services);
        return builder.Build();
    }

    private static void ConfigureDbContext(IServiceCollection services, GlobalConfig globalConfig, string[] args)
    {
        var isNeo4j = args.Any(a => a == "--neo");
        var isMongo = args.Any(a => a == "--mongo");

        if (isNeo4j)
        {
            services.AddSingleton<IDriver>(sp =>
            {
                var config = globalConfig.Neo4jConfig;
                return GraphDatabase.Driver(
                    config.Uri,
                    AuthTokens.Basic(config.User, config.Password));
            });
            services.AddScoped<krovNadGlavomNeo4jDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWorkNeo4j>();
        }
        else if (isMongo)
        {
            services.AddSingleton(new krovNadGlavomMongoDbContext(globalConfig.MongoDb));
            services.AddScoped<IUnitOfWork, UnitOfWorkMongo>();
        }
        else
        {
            services.AddDbContext<krovNadGlavomDbContext>(options =>
            {
                options.UseMySql(
                    globalConfig.ConnectionString,
                    ServerVersion.AutoDetect(globalConfig.ConnectionString),
                    mySqlOptionsAction: mySqlOptions =>
                    {
                        mySqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null
                        );
                    }
                );
            });
            services.AddScoped<IUnitOfWork, UnitOfWorkMySql>();
        }
    }

    private static void ConfigureAuthentication(IServiceCollection services, JWTSettings jWTSettings)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jWTSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jWTSettings.Audience,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTSettings.Secret)),
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    private static void ConfigureLogging(WebApplicationBuilder builder, GlobalConfig config)
    {
        builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(builder.Configuration));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: ALLOWED_SPECIFIC_ORIGINS_KEY,
                              builder =>
                              {
                                  builder
                                    .WithOrigins(config.AllowedOrigins.ToArray())
                                    .AllowCredentials()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .WithExposedHeaders("X-Pagination");
                              });
        });
    }

    private static void ConfigureDependencies(IServiceCollection services, GlobalConfig config)
    {
        services.AddMemoryCache();
        services.AddMediatR(cfg =>
        {
            cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzgzNzI4MDAwIiwiaWF0IjoiMTc1MjIyODM1NiIsImFjY291bnRfaWQiOiIwMTk3ZjhmMDk4M2Q3MjlmYWJjYmVmOGVlODUxYzcwMyIsImN1c3RvbWVyX2lkIjoiY3RtXzAxanp3ZjVyNHl2cXFyeTBnNzF0aHQzd2pqIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.iu-hEm3JyQtNYVAZw2lyykfTx-rEq3r7cC5nyfMFFJk4M1cPBawl-GEdNhrM2XvAgLDqlfTJDAekGtmmBYyN39qCBGjkExrxx_8M98v-Q6iiP759WfbF1NtSIOJnLP8ts9AJtMJL3bU7sCcbzYfmMlwc3NOXcyz3bu2qvIBEs-E4lkE0Ajx8DM-dJnXtLptL8EW675rDaKbPF6qTwxim6rXhVQs5HrHkyQOGSB0nJEBvK1A_ops8UVHpZ4ybKXlfThso8DKx6QRLQtufGinPk674529R6bnn1PSMez4nVJDO04JjulwDKEwQ8KSuPNVOHZCy-ueZUu3WMR4orpcs9Q";
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        services.AddSingleton(ac => config.JWTSettings);
        services.AddSingleton(ac => config.Neo4jConfig);
        services.AddSingleton(ac => config.EmailConfig);

        services.AddSingleton<ISecurePasswordHasher, SecurePasswordHasher>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<ICloudinaryService, CloudinaryService>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddScoped<IUserSessionService, UserSessionService>();
    }

    private static void ConfigureAutoMapper(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(MappingProfiles));
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.OperationFilter<AddRefreshTokenHeaderFilter>();

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
        });
    }

    private static void ConfigureFluentValidataion(IServiceCollection services)
    {
        // Registracija svih validatora
        services.AddValidatorsFromAssemblyContaining<AgencyToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<InstallmentProofToSendValidator>();
        services.AddValidatorsFromAssemblyContaining<AgencyRequestToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<AgencyRequestToUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<ApartmentToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<ApartmentToUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<BuildingToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<BuildingToUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<BuildingEndDateToExtendValidator>();
        services.AddValidatorsFromAssemblyContaining<ConstructionCompanyToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<ConstructionCompanyToUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<ContractToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<DiscountRequestToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<DiscountRequestToUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<GarageToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<GarageToUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<InstallmentToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<ReservationToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<UserAgencyFollowToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<UserToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<PriceListToAddValidator>();
        services.AddValidatorsFromAssemblyContaining<MultipleApartmentsToAddValidator>();

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
    }

    private static int ExitAndLogNoMatchingCommands(List<string> availableCommands)
    {
        Log.Error("Error: No command specified. Please provide a command.");
        Log.Error($"Available commands are {string.Join(", ", availableCommands)}");
        return 1;
    }
}