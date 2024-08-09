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
            var traceId = Guid.NewGuid();
            logger.LogError("Error occured while processing the request, TraceId : ${TraceId}, " +
                            "Message : ${ExMessage}, StackTrace: ${ExStackTrace}", traceId, notFound.Message, notFound.StackTrace);

            context.Response.StatusCode = StatusCodes.Status404NotFound;

            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Not Found",
                Status = (int)StatusCodes.Status404NotFound,
                Instance = context.Request.Path,
                Detail = $"Restaurant was not found, traceId : {traceId}",
            };
            await context.Response.WriteAsJsonAsync(problemDetails);

        }
        
        catch (Exception ex)
        {
            var traceId = Guid.NewGuid();
            logger.LogError("Error occured while processing the request, TraceId : ${TraceId}, " +
                            "Message : ${ExMessage}, StackTrace: ${ExStackTrace}", traceId, ex.Message, ex.StackTrace);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var problemDetails = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Internal Server Error",
                Status = (int)StatusCodes.Status500InternalServerError,
                Instance = context.Request.Path,
                Detail = $"Internal server error occured, traceId : {traceId}",
            };
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}