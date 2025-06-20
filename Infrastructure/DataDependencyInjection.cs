using Data.Data.Users;
using Data.Sfc;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class DataDependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services) =>
        services.AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<ISfcApi, SfcApi>();
}