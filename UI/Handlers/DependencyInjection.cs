using Microsoft.Extensions.DependencyInjection;

namespace UI.Handlers;

public static class DependencyInjection
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
        => services
            .AddSingleton<EventsHandler>()
            .AddTransient<OperationCreatedHandler>()
            .AddTransient<ShowSuccessHandler>()
            .AddTransient<ShowStopHandler>()
            .AddTransient<OpenWindowHandler>();
}