using Microsoft.Extensions.DependencyInjection;

namespace eStoreOnline.Application;

public static class ApplicationDependency
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services;
    }
}