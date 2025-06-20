using Domain.Core.Errors;
using OneOf;

namespace Domain.Logfiles;

public interface ILogfileGateway
{
    Task<OneOf<Logfile, Error>> UploadOperationAsync(
        Logfile logfileToUpload,
        int maxRetries,
        TimeSpan timeout,
        CancellationToken ct = default);
}