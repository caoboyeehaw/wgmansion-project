using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using WGMansion.Api.Settings;
using WGMansion.Api.Utility;
using WGMansion.Api.ViewModels;
using WGMansion.MongoDB.Services;
using WGMansion.MongoDB.Settings;

internal class Program
{
    private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));

    private static void Main(string[] args)
    {
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        _logger.Info($"--------Starting Server--------");

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Type = SecuritySchemeType.Http,
            });
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference =new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        builder.Services.AddSingleton(typeof(IMongoService<>), typeof(MongoService<>));
        builder.Services.AddTransient<ITokenGenerator, TokenGenerator>();
        builder.Services.AddTransient<IAccountsViewModel, AccountsViewModel>();
        builder.Services.AddTransient<ITickerViewModel, TickerViewModel>();
        builder.Services.AddTransient<ITickerHistoryViewModel, TickerHistoryViewModel>();

        builder.Configuration.AddUserSecrets<MongoSettings>();
        builder.Configuration.AddUserSecrets<AppSettings>();
        builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoDB"));
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    var key = Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"]);
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        _logger.Info($"--------Server Started--------");

        app.Run();
    }
}