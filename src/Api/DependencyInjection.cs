using Api.ErrorHandling;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        var currentAssembly = typeof(DependencyInjection).Assembly;
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddProblemDetails()    
            .AddExceptionHandler<FluentValidationExceptionHandler>()
            .AddExceptionHandler<GlobalExceptionHandler>();
        
        return services;
    }
}