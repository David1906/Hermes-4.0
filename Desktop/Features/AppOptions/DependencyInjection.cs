using Desktop.Core.Types;
using Desktop.Features.Logfiles.Domain;
using Desktop.Features.Machines.Domain;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Desktop.Features.AppOptions;

public static class DependencyInjection
{
    public static IServiceCollection AddAppOptions(this IServiceCollection services)
        => services
            .AddDomain()
            .AddUseCases()
            .AddDelivery()
            .AddInfrastructure();

    private static IServiceCollection AddDomain(this IServiceCollection services)
        => services
            .AddTransient<MachineOptions>()
            .AddTransient<GkgMachineOptions>()
            .AddSingleton<LogfilesGatewayOptions>(_ => new LogfilesGatewayOptions
            {
                BaseDirectory = new DirectoryInfo(@"C:\Users\david_ascencio\Documents\dev\Hermes\Sfc"),
                ResponseExtensionType = FileExtensionType.Log
            });

    private static IServiceCollection AddUseCases(this IServiceCollection services)
        => services;

    private static IServiceCollection AddDelivery(this IServiceCollection services)
        => services;

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services;
}