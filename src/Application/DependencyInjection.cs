using System.Reflection;
using Application.MediatorConfiguration;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddMediator();
            return services;
        }

        private IServiceCollection AddMediator()
        {
            var currentAssembly = typeof(DependencyInjection).Assembly;
        
            services.AddValidatorsFromAssembly(currentAssembly);
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(currentAssembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
        
            return services;
        }
    }
}