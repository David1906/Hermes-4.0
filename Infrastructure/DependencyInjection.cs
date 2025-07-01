using Domain.Logfiles;
using Domain.Machines;
using Domain.OperationTasks;
using Domain.Scanners;
using Infrastructure.Data.Logfiles;
using Infrastructure.Data.OperationTasks;
using Infrastructure.Data.Users;
using Infrastructure.Data;
using Infrastructure.Gateways;
using Infrastructure.Machines;
using Infrastructure.Scanners;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
        .AddScoped<IUsersRepository, UsersRepository>()
        .AddScoped<IOperationTasksRepository, OperationTasksRepository>()
        .AddScoped<ILogfilesRepository, LogfilesRepository>()
        .AddScoped<ILogfilesSfcGateway, LogfilesSfcGateway>()
        .AddScoped<ISerialScannerRx, SerialScannerRx>()
        .AddScoped<SqliteContext>()
        .AddScoped<IMachine, GkgMachine>();
}