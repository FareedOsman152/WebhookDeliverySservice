using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace Api.ErrorHandling;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService): IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        return exception switch
        {
            RetryLimitExceededException => await HandleDatabaseUnavailable(httpContext, exception),
            _ => await HandleInternalServerError(httpContext, exception)
        };
    }

    private async ValueTask<bool> HandleDatabaseUnavailable(HttpContext httpContext, Exception exception)
    {
        logger.LogError(exception, "Database unavailable. Request: {Method} {RequestPath}", httpContext.Request.Method, httpContext.Request.Path);
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status503ServiceUnavailable,
                Title = "Service Unavailable",
                Detail = "The database is unavailable. Try again later."
            }
        });
    }

    private async ValueTask<bool> HandleInternalServerError(HttpContext httpContext, Exception exception)
    {
        logger.LogError(exception, "Unhandled exception. Request: {Method} {RequestPath}", httpContext.Request.Method, httpContext.Request.Path);
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext =  httpContext,
            Exception = exception,
            ProblemDetails = new ValidationProblemDetails()
            {
                Status =  StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An internal server error occurred."
            }
        });
    }
}
