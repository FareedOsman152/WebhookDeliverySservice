using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.ErrorHandling;

public class FluentValidationExceptionHandler(ILogger<FluentValidationExceptionHandler> logger, IProblemDetailsService problemDetailsService): IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not ValidationException exc)
            return await ValueTask.FromResult(false);

        logger.LogError("Validation Error from request: {requestPath}, Error message: {message}", httpContext.Request.Path, exc.Message);
        
        var errors = exc.Errors.GroupBy(e => e.PropertyName).ToDictionary(k => k.Key, v => v.Select(e => e.ErrorMessage).ToArray());
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext =  httpContext,
            Exception = exc,
            ProblemDetails = new ValidationProblemDetails(errors)
            {
                Status =  StatusCodes.Status400BadRequest,
                Title =  exc.Message
            }
        });
    }
}