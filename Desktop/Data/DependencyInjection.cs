using Microsoft.Extensions.DependencyInjection;

namespace Desktop.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services)
        => services
            .AddSingleton<SqliteContext>();
}