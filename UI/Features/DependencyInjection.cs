using Microsoft.Extensions.DependencyInjection;
using UI.Features.AppOptions;
using UI.Features.Main;
using UI.Features.Operations;
using UI.Features.Panels;
using UI.Features.Stops;

namespace UI.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
        => services
            .AddTransient<MainWindowViewModel>()
            .AddTransient<PanelProcessorViewModel>()
            .AddTransient<SuccessViewModel>()
            .AddTransient<StopViewModel>()
            .AddAppOptions();
}