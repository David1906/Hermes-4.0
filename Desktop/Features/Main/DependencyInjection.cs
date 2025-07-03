using Desktop.Features.Main.Delivery;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Features.Main;

public static class DependencyInjection
{
    public static IServiceCollection AddMain(this IServiceCollection services)
        => services
            .AddUseCases()
            .AddDelivery()
            .AddInfrastructure();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
        => services;

    private static IServiceCollection AddDelivery(this IServiceCollection services)
        => services
            .AddTransient<MainWindowViewModel>();

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services;
}