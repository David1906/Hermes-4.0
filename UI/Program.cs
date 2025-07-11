using System;
using Avalonia;
using Core.Application;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Paramore.Brighter.Extensions.DependencyInjection;
using UI.Common;
using UI.Features;
using UI.Machines;

namespace UI;

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
            .AddMachines()
            .AddFeatures()
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