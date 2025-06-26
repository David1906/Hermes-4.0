using Common.ResultOf;

namespace Domain.Logfiles;

public interface ILogfileGateway
{
    Task<ResultOf<Logfile>> UploadOperationAsync(
        Logfile logfileToUpload,
        int maxRetries,
        TimeSpan timeout,
        CancellationToken ct = default);
}