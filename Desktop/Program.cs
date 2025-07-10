using Avalonia;
using Desktop.Common;
using Desktop.Data;
using Desktop.Features;
using Desktop.Gateways;
using Microsoft.Extensions.Hosting;
using Paramore.Brighter.Extensions.DependencyInjection;
using System;
using Core.Application;
using Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Desktop;

sealed class Program
{
    public static IHost AppHost { get; private set; } = null!;

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var applicationBuilder = Host.CreateApplicationBuilder(args);
        applicationBuilder.Services
            .AddApplication()
            .AddInfrastructure()
            .AddCommon()
            .AddFeatures()
            .AddData()
            .AddGateways()
            .AddBrighter()
            .AutoFromAssemblies();

        applicationBuilder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("Assets/appsettings.json", optional: true, reloadOnChange: true);

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