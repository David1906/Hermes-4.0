using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Handlers;

public static class DependencyInjection
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
        => services
            .AddSingleton<EventsHandler>()
            .AddTransient<OperationCreatedHandler>()
            .AddTransient<OperationTaskCreatedHandler>()
            .AddTransient<OpenWindowHandler>();
}