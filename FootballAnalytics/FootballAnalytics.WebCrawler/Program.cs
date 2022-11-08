using FootballAnalytics.Infrastructure.Configuration;
using FootballAnalytics.WebCrawler;

IHost host = Host.CreateDefaultBuilder(args)
    .AddCommonConfiguration()
    .ConfigureServices((context, services) => DependencyInjectionSetup.RegisterServices(services, context))
    .Build();

await host.RunAsync();