using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace FootballAnalytics.Infrastructure.Configuration;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddCommonConfiguration(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            var env = hostingContext.HostingEnvironment;
            var configurationFolder = Path.Combine(env.ContentRootPath, "..", "Configuration");
            config.AddJsonFile(Path.Combine(configurationFolder, "common.appsettings.json"), optional: true) // Local
                .AddJsonFile("common.appsettings.json", optional: true); // Published
        });
    }
}