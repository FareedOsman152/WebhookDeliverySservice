using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Shared;

namespace Application.MediatorConfiguration;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators): IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!validators.Any())
            return await next(ct); 
        
        var ctx = new ValidationContext<TRequest>(request);
        
        var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(ctx, ct)));
        var failures = results.SelectMany(r => r.Errors).ToList();
        if (failures.Count != 0)
            throw new FluentValidation.ValidationException(failures);
        
        return await next(ct);
    }
}