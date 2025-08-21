using Microsoft.Extensions.DependencyInjection;
using UI.Features.AppOptions;
using UI.Features.Main;
using UI.Features.Panels;
using UI.Features.Stops;
using PanelProcessorViewModel = UI.Features.Panels.PanelProcessorViewModel;

namespace UI.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        return services
            .AddTransient<MainWindowViewModel>()
            .AddTransient<PanelProcessorViewModel>()
            .AddTransient<SuccessViewModel>()
            .AddTransient<StopViewModel>()
            .AddAppOptions();
    }
}