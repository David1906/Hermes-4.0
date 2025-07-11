using Common.ResultOf;
using Core.Domain;

namespace Core.Application.Common.Gateways;

public interface ISfcSharedFolderGateway
{
    Task<ResultOf<Logfile>> SendPanelToNextStationAsync(
        Logfile logfileToUpload,
        int maxRetries,
        TimeSpan timeout,
        CancellationToken ct = default);
}