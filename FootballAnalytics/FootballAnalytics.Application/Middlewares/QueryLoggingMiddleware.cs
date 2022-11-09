using Microsoft.Extensions.Logging;

namespace FootballAnalytics.Application.Middlewares;

public sealed class QueryLoggingMiddleware : IQueryMiddleware
{
    private readonly ILoggerFactory _loggerFactory;

    public QueryLoggingMiddleware(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public async Task<TResponse> Execute<TQuery, TResponse>(QueryMiddlewareContext<TQuery, TResponse> ctx)
        where TQuery : class
    {
        var logger = _loggerFactory.CreateLogger($"QueryHandler[{typeof(TQuery).Name},{typeof(TResponse).Name}]");
        var queryId = Guid.NewGuid();

        logger.LogInformation("[{QueryId}] Handling query of type {QueryType}", queryId, typeof(TQuery).Name);

        var response = await ctx.Next(ctx.Query, ctx.CancellationToken);
        
        logger.LogInformation("[{QueryId}] Handled query of type {QueryType}", queryId, typeof(TQuery).Name);
        
        return response;
    }
}

public static class LoggingQueryPipelineBuilderExtensions
{
    public static IQueryPipelineBuilder UseLogging(this IQueryPipelineBuilder pipeline)
    {
        return pipeline.Use<QueryLoggingMiddleware>();
    }
}