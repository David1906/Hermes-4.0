using Desktop.Features.Locking;
using Desktop.Features.Main;
using Desktop.Features.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
        => services
            .AddSingleton<ViewModelFactory>()
            .AddTransient<MainWindowViewModel>()
            .AddTransient<StopViewModel>()
            .AddTransient<OperationProcessorViewModel>();
}