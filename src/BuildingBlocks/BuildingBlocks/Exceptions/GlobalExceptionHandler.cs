using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var result = new ProblemDetails();
        switch (exception)
        {
            case ArgumentNullException argumentNullException:
                result = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Type = argumentNullException.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = argumentNullException.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                };
                logger.LogError(argumentNullException, $"Exception occured : {argumentNullException.Message}");
                break;
            default:
                result = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = exception.GetType().Name,
                    Title = "An unexpected error occurred",
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                };
                logger.LogError(exception, $"Exception occured : {exception.Message}");
                break;
        }
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken);
        return true;
    }
}