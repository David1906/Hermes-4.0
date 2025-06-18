using Microsoft.Extensions.DependencyInjection;
using UseCases.Users;

namespace UseCases;

public static class UseCasesDependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
        => services.AddUserUseCases();

    private static IServiceCollection AddUserUseCases(this IServiceCollection services)
        => services.AddScoped<UserUseCases>()
            .AddScoped<AddUser>();
}