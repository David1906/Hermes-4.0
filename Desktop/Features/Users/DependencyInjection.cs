using Desktop.Features.Stops.Delivery;
using Desktop.Features.Users.Delivery;
using Desktop.Features.Users.Domain;
using Desktop.Features.Users.Infrastructure;
using Desktop.Features.Users.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Features.Users;

public static class DependencyInjection
{
    public static IServiceCollection AddUsers(this IServiceCollection services)
        => services
            .AddUseCases()
            .AddDelivery()
            .AddInfrastructure();

    private static IServiceCollection AddUseCases(this IServiceCollection services)
        => services
            .AddTransient<UsersUseCases>()
            .AddTransient<AddUserUseCase>();

    private static IServiceCollection AddDelivery(this IServiceCollection services)
        => services
            .AddTransient<UserViewModel>();

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddTransient<IUsersRepository, UsersRepository>();
}