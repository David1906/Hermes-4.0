using Core.Application.Common.Data;
using Core.Application.Common.FileParsers;
using Core.Application.Common.Gateways;
using Infrastructure.Data.Features.Logfiles;
using Infrastructure.Data.Features.Panels;
using Infrastructure.Data;
using Infrastructure.FileParsers;
using Infrastructure.Gateways;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddData()
            .AddFileParser()
            .AddGateways();

    private static IServiceCollection AddData(this IServiceCollection services)
        => services
            .AddSingleton<HermesContext>()
            .AddSingleton<IUnitOfWork, UnitOfWork>()
            .AddTransient<IPanelsRepository, PanelsRepository>()
            .AddTransient<ILogfilesRepository, LogfilesRepository>();

    private static IServiceCollection AddFileParser(this IServiceCollection services)
        => services
            .AddSingleton<IPanelParser, PanelParser>()
            .AddSingleton<IOperationParser, OperationParser>()
            .AddSingleton<ISfcResponseOperationParser, SfcResponseOperationParser>()
            .AddSingleton<TriMachineOperationParser>()
            .AddSingleton<SfcResponseOperationParser>();

    private static IServiceCollection AddGateways(this IServiceCollection services)
        => services
            .AddTransient<SfcSharedFolderGatewayOptions>()
            .AddSingleton<ISfcSharedFolderGateway, SfcSharedFolderGateway>();
}