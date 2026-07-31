using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger) : IMiddleware
{
    private const int SlowRequestThresholdMs = 4000;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopWatch = Stopwatch.StartNew();
        await next.Invoke(context);
        stopWatch.Stop();

        if (stopWatch.ElapsedMilliseconds > SlowRequestThresholdMs)
        {
            logger.LogInformation("Request [{Verb}] at {Path} took {Time} ms",
                context.Request.Method,
                context.Request.Path,
                stopWatch.ElapsedMilliseconds);
        }
    }
}
