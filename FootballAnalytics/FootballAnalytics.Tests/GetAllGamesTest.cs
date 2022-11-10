using Conqueror.CQS.Transport.Http.Client;
using FootballAnalytics.Application.GetAllGames;
using FootballAnalytics.Domain.Entities;
using FootballAnalytics.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace FootballAnalytics.Tests;

[TestClass]
public class GetAllGamesTest
{
    private WebApplicationFactory<Program> _applicationFactory;
    private HttpClient _client;

    [TestMethod]
    public async Task Given_When_Then()
    {
        _applicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b => b.ConfigureServices(ConfigureServerServices));

        _client = _applicationFactory.CreateClient();

        var getAllGamesQueryHandler = CreateQueryHttpClient<IGetAllGamesQueryHandler>();

        var result = await getAllGamesQueryHandler.ExecuteQuery(new());

        Assert.AreEqual(1, result.Games.Count);
    }

    private T Resolve<T>()
        where T : notnull => _applicationFactory.Services.GetRequiredService<T>();

    private T ResolveOnClient<T>()
        where T : notnull
    {
        var services = new ServiceCollection();
        ConfigureClientServices(services);
        var provider = services.FinalizeConquerorRegistrations().BuildServiceProvider();
        return provider.GetRequiredService<T>();
    }

    private void ConfigureClientServices(IServiceCollection services)
    {
        services.AddConquerorCQSHttpClientServices();
    }
    
    private void ConfigureServerServices(IServiceCollection services)
    {
        var repoMock = new Mock<IGetAllGamesQueryHandlerRepository>();
        repoMock.Setup(r => r.GetAllGames(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Game>
            {
                new()
            });

        services.AddSingleton<IGetAllGamesQueryHandlerRepository>(repoMock.Object);
        services.Replace(ServiceDescriptor.Singleton<IGetAllGamesQueryHandlerRepository>(_ => repoMock.Object));
    }

    private THandler CreateCommandHttpClient<THandler>()
        where THandler : class, ICommandHandler => ResolveOnClient<ICommandClientFactory>()
        .CreateCommandClient<THandler>(b => b.UseHttp(_client));

    private THandler CreateQueryHttpClient<THandler>()
        where THandler : class, IQueryHandler => ResolveOnClient<IQueryClientFactory>()
        .CreateQueryClient<THandler>(b => b.UseHttp(_client));
}