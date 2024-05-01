using eStoreOnline.Application.Implementations;
using eStoreOnline.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace eStoreOnline.Application;

public static class ApplicationDependency
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IStorageService, LocalFileStorageService>();
        return services;
    }
}