using Desktop.Features.Stops.Delivery;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Features.Stops;

public static class DependencyInjection
{
    public static IServiceCollection AddStops(this IServiceCollection services)
        => services
            .AddUseCases()
            .AddDelivery()
            .AddInfrastructure();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
        => services;

    private static IServiceCollection AddDelivery(this IServiceCollection services)
        => services
            .AddTransient<StopViewModel>();

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services;
}