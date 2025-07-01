using Avalonia;
using System;
using System.IO;
using Common;
using Common.Serial;
using Desktop.ViewModels;
using Domain.Core.Types;
using Domain.Logfiles;
using Domain.Machines;
using Domain.Operations;
using Infrastructure;
using Infrastructure.Scanners;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UseCases;

namespace Desktop;

sealed class Program
{
    public static IHost AppHost { get; set; } = null!;

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var applicationBuilder = Host.CreateApplicationBuilder(args);
        applicationBuilder.Services
            .AddViewModels()
            .AddInfrastructure()
            .AddUseCases();

        // TODO
        applicationBuilder.Services.AddTransient<MachineOptions>();
        applicationBuilder.Services.AddTransient<GkgMachineOptions>();
        applicationBuilder.Services.AddTransient<SerialPortAsync>();
        applicationBuilder.Services.AddTransient<SerialPortRx>();
        applicationBuilder.Services.AddTransient<SerialScannerRx>();
        applicationBuilder.Services.AddTransient<TriOperationParser>();
        applicationBuilder.Services.AddSingleton<IResilientFileSystem, ResilientFileSystem>();
        applicationBuilder.Services.AddTransient<IFileSystemWatcherRx, FileSystemWatcherRx>();
        applicationBuilder.Services.AddSingleton<LogfilesGatewayOptions>(_ =>
        {
            return new LogfilesGatewayOptions
            {
                BaseDirectory = new DirectoryInfo(@"C:\Users\david_ascencio\Documents\dev\Hermes\Sfc"),
                ResponseExtensionType = FileExtensionType.Log
            };
        });
        applicationBuilder.Services.AddSingleton<TimeProvider>(_ => TimeProvider.System);

        AppHost = applicationBuilder.Build();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}