using eStoreOnline.Infrastructure.Configurations;
using eStoreOnline.Infrastructure.Implementations;
using eStoreOnline.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eStoreOnline.Infrastructure;

public static class InfrastructureDependency
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPaymentGatewayService, StripePaymentGatewayService>();
        services.AddScoped<IIdentityPrincipal, IdentityPrincipal>();
        services.Configure<StripeConfiguration>(configuration.GetSection(StripeConfiguration.SectionName));

        return services;
    }
}