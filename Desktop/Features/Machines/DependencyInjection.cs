using Desktop.Features.Machines.Domain;
using Desktop.Features.Machines.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Features.Machines;

public static class DependencyInjection
{
    public static IServiceCollection AddMachines(this IServiceCollection services)
        => services
            .AddTransient<IMachine, GkgMachine>();
}