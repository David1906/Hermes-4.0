using Desktop.Features.Logfiles.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Gateways;

public static class DependencyInjection
{
    public static IServiceCollection AddGateways(this IServiceCollection services)
        => services
            .AddSingleton<ILogfilesSfcGateway, LogfilesSfcGateway>();
}