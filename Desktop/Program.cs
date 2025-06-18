using Avalonia;
using System;
using Data;
using Desktop.ViewModels;
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
        var appHostBuilder = Host.CreateApplicationBuilder(args);
        appHostBuilder.Services
            .AddViewModels()
            .AddData()
            .AddUseCases();

        AppHost = appHostBuilder.Build();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}