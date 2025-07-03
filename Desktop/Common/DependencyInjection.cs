using System;
using Common;
using Common.Serial;
using Desktop.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
        => services
            .AddHandlers()
            .AddTransient<ViewModelFactory>()
            .AddTransient<SerialPortAsync>()
            .AddTransient<SerialPortRx>()
            .AddSingleton<IResilientFileSystem, ResilientFileSystem>()
            .AddTransient<IFileSystemWatcherRx, FileSystemWatcherRx>()
            .AddSingleton<TimeProvider>(_ => TimeProvider.System);
}