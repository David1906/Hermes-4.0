using Desktop.Features.OperationTasks.Domain;
using Desktop.Features.OperationTasks.Infrastructure;
using Desktop.Features.OperationTasks.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Features.OperationTasks;

public static class DependencyInjection
{
    public static IServiceCollection AddOperationTasks(this IServiceCollection services)
        => services
            .AddUseCases()
            .AddDelivery()
            .AddInfrastructure();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
        => services
            .AddTransient<OperationsTasksUseCases>()
            .AddTransient<AddOperationTaskUseCase>();

    private static IServiceCollection AddDelivery(this IServiceCollection services)
        => services;

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddTransient<IOperationTasksRepository, OperationTasksRepository>();
}