using Core.Application.Common.Data;
using Core.Application.Common.ExternalDevices;
using Core.Application.Common.FileBuilders;
using Core.Application.Common.FileParsers;
using Core.Application.Common.Gateways;
using Infrastructure.Data.Features.Logfiles;
using Infrastructure.Data.Features.Panels;
using Infrastructure.Data;
using Infrastructure.ExternalDevices;
using Infrastructure.FileBuilders;
using Infrastructure.FileParsers;
using Infrastructure.Gateways;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddExternalDevices()
            .AddData()
            .AddFileBuilders()
            .AddFileParsers()
            .AddGateways();

    private static IServiceCollection AddExternalDevices(this IServiceCollection services)
        => services
            .AddSingleton<ISerialScannerRx, SerialScannerRx>();

    private static IServiceCollection AddData(this IServiceCollection services)
        => services
            .AddSingleton<HermesContext>()
            .AddSingleton<IUnitOfWork, UnitOfWork>()
            .AddTransient<IPanelsRepository, PanelsRepository>()
            .AddTransient<ILogfilesRepository, LogfilesRepository>();

    private static IServiceCollection AddFileBuilders(this IServiceCollection services)
        => services
            .AddSingleton<ITriLogFileBuilder, TriLogFileBuilder>();

    private static IServiceCollection AddFileParsers(this IServiceCollection services)
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