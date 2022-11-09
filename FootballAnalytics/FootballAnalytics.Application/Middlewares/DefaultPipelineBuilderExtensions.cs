namespace FootballAnalytics.Application.Middlewares;

public static class DefaultPipelineBuilderExtensions
{
    public static ICommandPipelineBuilder UseDefault(this ICommandPipelineBuilder pipeline)
    {
        return pipeline.UseLogging();
    }
    
    public static IQueryPipelineBuilder UseDefault(this IQueryPipelineBuilder pipeline)
    {
        return pipeline.UseLogging();
    }
}