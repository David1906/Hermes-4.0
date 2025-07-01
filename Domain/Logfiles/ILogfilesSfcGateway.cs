using Common.ResultOf;

namespace Domain.Logfiles;

public interface ILogfilesSfcGateway
{
    Task<ResultOf<Logfile>> UploadOperationAsync(
        Logfile logfileToUpload,
        int maxRetries,
        TimeSpan timeout,
        CancellationToken ct = default);
}