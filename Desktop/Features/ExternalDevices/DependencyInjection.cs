using Desktop.Features.ExternalDevices.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Features.ExternalDevices;

public static class DependencyInjection
{
    public static IServiceCollection AddExternalDevices(this IServiceCollection services)
        => services
            .AddUseCases()
            .AddDelivery()
            .AddInfrastructure();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
        => services;

    private static IServiceCollection AddDelivery(this IServiceCollection services)
        => services;

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddTransient<SerialScannerRx>();
}