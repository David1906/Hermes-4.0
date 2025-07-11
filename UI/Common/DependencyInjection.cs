using System;
using Common;
using Common.Serial;
using Microsoft.Extensions.DependencyInjection;
using UI.Handlers;

namespace UI.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
        => services
            .AddHandlers()
            .AddTransient<SerialPortAsync>()
            .AddTransient<SerialPortRx>()
            .AddSingleton<IResilientFileSystem, ResilientFileSystem>()
            .AddTransient<IFileSystemWatcherRx, FileSystemWatcherRx>()
            .AddSingleton<TimeProvider>(_ => TimeProvider.System);
}