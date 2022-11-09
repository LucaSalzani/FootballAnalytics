using Conqueror;

namespace FootballAnalytics.Application.Middlewares;

public sealed class CommandLoggingMiddleware : ICommandMiddleware
{
    public Task<TResponse> Execute<TCommand, TResponse>(CommandMiddlewareContext<TCommand, TResponse> ctx)
        where TCommand : class
    {
        // TODO: Implement logger
        return ctx.Next(ctx.Command, ctx.CancellationToken);
    }
}

public static class LoggingCommandPipelineBuilderExtensions
{
    public static ICommandPipelineBuilder UseLogging(this ICommandPipelineBuilder pipeline)
    {
        return pipeline.Use<CommandLoggingMiddleware>();
    }
}