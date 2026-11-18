using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.ErrorHandling;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService): IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
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