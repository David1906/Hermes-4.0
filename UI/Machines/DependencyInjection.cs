using Microsoft.Extensions.DependencyInjection;

namespace UI.Machines;

public static class DependencyInjection
{
    public static IServiceCollection AddMachines(this IServiceCollection services)
        => services
            .AddTransient<IMachine, GkgMachine>();
}