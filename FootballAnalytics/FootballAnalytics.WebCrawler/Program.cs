using FootballAnalytics.WebCrawler;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        var env = hostingContext.HostingEnvironment;
        var configurationFolder = Path.Combine(env.ContentRootPath, "..", "Configuration");

        config.AddJsonFile(Path.Combine(configurationFolder, "common.appsettings.json"),
                optional: true) // When running using dotnet run
            .AddJsonFile("common.appsettings.json", optional: true); // When app is published
    })
    .ConfigureServices((context, services) => DependencyInjectionSetup.RegisterServices(services, context))
    .Build();

await host.RunAsync();
