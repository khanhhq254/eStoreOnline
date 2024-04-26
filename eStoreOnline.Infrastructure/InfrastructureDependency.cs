using eStoreOnline.Infrastructure.Implementations;
using eStoreOnline.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace eStoreOnline.Infrastructure;

public static class InfrastructureDependency 
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPaymentGatewayService, StripePaymentGatewayService>();
        return services;
    }
}