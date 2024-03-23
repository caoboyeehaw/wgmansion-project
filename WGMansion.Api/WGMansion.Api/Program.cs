using log4net;
using log4net.Config;
using System.Reflection;
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
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped(typeof(IMongoService<>), typeof(MongoService<>));
        builder.Services.AddTransient<IAccountsViewModel, AccountsViewModel>();
        builder.Services.AddTransient<ITokenGenerator, TokenGenerator>();

        builder.Configuration.AddUserSecrets<MongoSettings>();
        builder.Configuration.AddUserSecrets<AppSettings>();
        builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoDB"));
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        _logger.Info($"--------Server Started--------");

        app.Run();
    }
}