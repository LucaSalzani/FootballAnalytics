using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Infrastructure;
using FootballAnalytics.WebCrawler;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IGameRepository, GameRepository>();
        services.AddSingleton<IFvrzWebService, FvrzWebService>();
    })
    .Build();

await host.RunAsync();
