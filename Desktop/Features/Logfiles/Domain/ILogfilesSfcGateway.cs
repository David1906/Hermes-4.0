using System;
using System.Threading;
using System.Threading.Tasks;
using Common.ResultOf;

namespace Desktop.Features.Logfiles.Domain;

public interface ILogfilesSfcGateway
{
    Task<ResultOf<Logfile>> UploadOperationAsync(
        Logfile logfileToUpload,
        int maxRetries,
        TimeSpan timeout,
        CancellationToken ct = default);
}