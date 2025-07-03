using Desktop.Features.Logfiles.Domain;
using Desktop.Features.Logfiles.Infrastructure;
using Desktop.Features.Logfiles.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Features.Logfiles;

public static class DependencyInjection
{
    public static IServiceCollection AddLogfiles(this IServiceCollection services)
        => services
            .AddUseCases()
            .AddDelivery()
            .AddInfrastructure();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
        => services.AddTransient<LogfilesUseCases>()
            .AddTransient<AddLogfileToSfcUseCase>()
            .AddTransient<MoveLogfileToBackup>();

    private static IServiceCollection AddDelivery(this IServiceCollection services)
        => services;

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddTransient<ILogfilesRepository, LogfilesRepository>();
}