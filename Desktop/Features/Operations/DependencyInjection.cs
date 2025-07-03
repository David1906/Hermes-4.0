using Desktop.Features.Operations.Delivery;
using Desktop.Features.Operations.Domain;
using Desktop.Features.Operations.Infrastructure;
using Desktop.Features.Operations.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Features.Operations;

public static class DependencyInjection
{
    public static IServiceCollection AddOperations(this IServiceCollection services)
        => services
            .AddDomain()
            .AddUseCases()
            .AddDelivery()
            .AddInfrastructure();

    private static IServiceCollection AddDomain(this IServiceCollection services)
        => services
            .AddTransient<TriOperationParser>();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
        => services
            .AddTransient<OperationsUseCases>()
            .AddTransient<ProcessOperationUseCase>()
            .AddTransient<AddOperationUseCase>();

    private static IServiceCollection AddDelivery(this IServiceCollection services)
        => services
            .AddTransient<OperationProcessorViewModel>()
            .AddTransient<SuccessViewModel>();

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddTransient<IOperationsRepository, OperationsRepository>();
}