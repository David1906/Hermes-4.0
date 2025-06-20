using Data.Data.Users;
using Data.Gateways;
using Data.Machines;
using Data.Scanners;
using Domain.Logfiles;
using Domain.Machines;
using Domain.Scanners;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
        .AddScoped<IUsersRepository, UsersRepository>()
        .AddScoped<ILogfileGateway, LogfileGateway>()
        .AddScoped<ISerialScannerRx, SerialScannerRx>()
        .AddScoped<IMachine, GkgMachine>();
}