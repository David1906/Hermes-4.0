using Desktop.Features.AppOptions;
using Desktop.Features.ExternalDevices;
using Desktop.Features.Logfiles;
using Desktop.Features.Machines;
using Desktop.Features.Main;
using Desktop.Features.OperationTasks;
using Desktop.Features.Operations;
using Desktop.Features.Stops;
using Desktop.Features.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
        => services
            .AddExternalDevices()
            .AddLogfiles()
            .AddMachines()
            .AddMain()
            .AddOperations()
            .AddOperationTasks()
            .AddAppOptions()
            .AddStops()
            .AddUsers();
}