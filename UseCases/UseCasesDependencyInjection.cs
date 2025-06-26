using Microsoft.Extensions.DependencyInjection;
using UseCases.Logfiles;
using UseCases.Operations;
using UseCases.Users;

namespace UseCases;

public static class UseCasesDependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
        => services.AddUserUseCases()
            .AddLogfilesUseCases()
            .AddOperationsUseCases();

    private static IServiceCollection AddLogfilesUseCases(this IServiceCollection services)
        => services.AddTransient<LogfilesUseCases>()
            .AddTransient<AddLogfileToSfc>()
            .AddTransient<MoveLogfileToBackup>();

    private static IServiceCollection AddOperationsUseCases(this IServiceCollection services) => services
        .AddTransient<OperationsUseCases>()
        .AddTransient<AddOperation>();

    private static IServiceCollection AddUserUseCases(this IServiceCollection services)
        => services.AddScoped<UserUseCases>()
            .AddScoped<AddUser>();
}