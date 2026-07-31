using Microsoft.AspNetCore.Mvc;
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFound)
        {
            await HandleExceptionAsync(context, notFound, StatusCodes.Status404NotFound, "Not Found", notFound.Message, LogLevel.Warning);
        }
        catch (ForbiddenException forbidden)
        {
            await HandleExceptionAsync(context, forbidden, StatusCodes.Status403Forbidden, "Forbidden", forbidden.Message, LogLevel.Warning);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred", LogLevel.Error);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string title, string detail, LogLevel logLevel)
    {
        var traceId = context.TraceIdentifier;
        logger.Log(logLevel, exception, "Error occurred while processing the request, TraceId: {TraceId}", traceId);

        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = title,
            Status = statusCode,
            Instance = context.Request.Path,
            Detail = $"{detail}, traceId : {traceId}",
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
