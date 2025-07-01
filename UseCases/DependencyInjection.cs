using Microsoft.Extensions.DependencyInjection;
using UseCases.Logfiles;
using UseCases.Operations;
using UseCases.OperationTasks;
using UseCases.Users;

namespace UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
        => services.AddUserUseCases()
            .AddLogfilesUseCases()
            .AddOperationsUseCases()
            .AddOperationTasksUseCases();

    private static IServiceCollection AddLogfilesUseCases(this IServiceCollection services)
        => services.AddTransient<LogfilesUseCases>()
            .AddTransient<AddLogfileToSfcUseCase>()
            .AddTransient<MoveLogfileToBackup>();

    private static IServiceCollection AddOperationsUseCases(this IServiceCollection services) => services
        .AddTransient<OperationsUseCases>()
        .AddTransient<ProcessOperationUseCase>()
        .AddTransient<AddOperationUseCase>();

    private static IServiceCollection AddOperationTasksUseCases(this IServiceCollection services) => services
        .AddTransient<OperationsTasksUseCases>()
        .AddTransient<AddOperationTaskUseCase>();

    private static IServiceCollection AddUserUseCases(this IServiceCollection services)
        => services.AddScoped<UserUseCases>()
            .AddScoped<AddUser>();
}