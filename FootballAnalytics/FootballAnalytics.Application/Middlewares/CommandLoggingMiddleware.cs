using Microsoft.Extensions.Logging;

namespace FootballAnalytics.Application.Middlewares;

public sealed class CommandLoggingMiddleware : ICommandMiddleware
{
    private readonly ILoggerFactory _loggerFactory;

    public CommandLoggingMiddleware(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public async Task<TResponse> Execute<TCommand, TResponse>(CommandMiddlewareContext<TCommand, TResponse> ctx)
        where TCommand : class
    {
        var logger = _loggerFactory.CreateLogger($"CommandHandler[{typeof(TCommand).Name},{typeof(TResponse).Name}]");
        var commandId = Guid.NewGuid();
        
        logger.LogInformation("[{CommandId}] Handling command of type {CommandType}", commandId, typeof(TCommand).Name);

        var response = await ctx.Next(ctx.Command, ctx.CancellationToken);
        
        logger.LogInformation("[{CommandId}] Handled command of type {CommandType}", commandId, typeof(TCommand).Name);
        return response;
    }
}

public static class LoggingCommandPipelineBuilderExtensions
{
    public static ICommandPipelineBuilder UseLogging(this ICommandPipelineBuilder pipeline)
    {
        return pipeline.Use<CommandLoggingMiddleware>();
    }
}