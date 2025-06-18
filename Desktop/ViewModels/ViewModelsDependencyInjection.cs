using Microsoft.Extensions.DependencyInjection;

namespace Desktop.ViewModels;

public static class ViewModelsDependencyInjection
{
    public static IServiceCollection AddViewModels(this IServiceCollection services) =>
        services.AddTransient<MainWindowViewModel>();
}