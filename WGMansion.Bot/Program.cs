using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;
using WGMansion.Bot.Business;
using WGMansion.Bot.Services;
using WGMansion.Bot.Settings;
internal class Program
{
    private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));

    static void Main(string[] args)
    {
        try
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var builder = Setup(args);
            using var host = builder.Build();

            var apiService = host.Services.GetRequiredService<IApiService>();
            var botSettings = host.Services.GetRequiredService<IOptions<BotSettings>>();
            var bot = new TradeBot(apiService, botSettings);
            bot.Start();
            host.Run();
        }
        catch (Exception e)
        {
            _logger.Error(e);
        }
    }

    private static HostApplicationBuilder Setup(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHttpClient();
        builder.Services.AddScoped<IApiService, ApiService>();

        builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
        builder.Services.Configure<BotSettings>(builder.Configuration.GetSection("BotSettings"));

        return builder;
    }
}