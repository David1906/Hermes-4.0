using Core.Application.Features.Logfiles.Commands;
using Core.Application.Features.Panels.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddLogfiles()
            .AddPanels();

    private static IServiceCollection AddLogfiles(this IServiceCollection services)
        => services
            .AddTransient<MoveLogfileToBackupHandler>();

    private static IServiceCollection AddPanels(this IServiceCollection services)
        => services
            .AddTransient<ProcessPanelFromLogfileHandler>()
            .AddTransient<CreatePanelFromLogfileHandler>()
            .AddTransient<SendPanelToNextStationHandler>();
}