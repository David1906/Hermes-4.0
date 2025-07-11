using Core.Application.Common.Gateways;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Core.Domain.Common.Types;
using UI.Machines;

namespace UI.Features.AppOptions;

public static class DependencyInjection
{
    public static IServiceCollection AddAppOptions(this IServiceCollection services)
        => services
            .AddTransient<MachineOptions>()
            .AddTransient<GkgMachineOptions>()
            .AddSingleton<SfcSharedFolderGatewayOptions>(_ => new SfcSharedFolderGatewayOptions
            {
                BaseDirectory = new DirectoryInfo(@"C:\Users\david_ascencio\Documents\dev\Hermes\Sfc"),
                ResponseExtensionType = FileExtensionType.Log
            });
}