using Application.Marketplaces;
using Application.Order;
using Application.Shippers;
using Application.ShippingContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Marketplaces;
using Persistence.Order;
using Persistence.Shippers;
using Persistence.ShippingContracts;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionString"];

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
        {
            sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
        }));

        services.AddScoped<IMarketplaceOrderRepository, MarketplaceOrderRepository>();
        services.AddScoped<IMarketplaceRepository, MarketplaceRepository>();
        services.AddScoped<IShipperRepository, ShipperRepository>();
        services.AddScoped<IShippingContractRepository, ShippingContractRepository>();
        return services;
    }
}