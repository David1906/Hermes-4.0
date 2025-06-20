using Microsoft.Extensions.DependencyInjection;
using UseCases.Operations;
using UseCases.Users;

namespace UseCases;

public static class UseCasesDependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
        => services.AddUserUseCases()
            .AddOperationsUseCases();

    private static IServiceCollection AddOperationsUseCases(this IServiceCollection services)
        => services.AddTransient<OperationsUseCases>()
            .AddTransient<AddOperationToSfc>();

    private static IServiceCollection AddUserUseCases(this IServiceCollection services)
        => services.AddScoped<UserUseCases>()
            .AddScoped<AddUser>();
}