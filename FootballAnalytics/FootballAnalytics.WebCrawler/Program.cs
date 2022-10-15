using FootballAnalytics.WebCrawler;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(DependencyInjectionSetup.RegisterServices)
    .Build();

await host.RunAsync();
