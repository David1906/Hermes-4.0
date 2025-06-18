using Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DataDependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services) =>
        services.AddScoped<IUsersRepository, UsersRepository>();
}