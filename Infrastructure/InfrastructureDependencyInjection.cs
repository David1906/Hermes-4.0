using Domain.Logfiles;
using Domain.Machines;
using Domain.Scanners;
using Infrastructure.Data;
using Infrastructure.Data.Users;
using Infrastructure.Gateways;
using Infrastructure.Machines;
using Infrastructure.Scanners;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
        .AddScoped<IUsersRepository, UsersRepository>()
        .AddScoped<ILogfileGateway, LogfileGateway>()
        .AddScoped<ISerialScannerRx, SerialScannerRx>()
        .AddScoped<SqliteContext>()
        .AddScoped<IMachine, GkgMachine>();
}