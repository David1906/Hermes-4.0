using Common;
using Common.Extensions;
using Domain.Logfiles;
using OneOf;
using R3;
using Error = Domain.Core.Errors.Error;

namespace Infrastructure.Gateways;

public class LogfileGateway(
    LogfileGatewayOptions options,
    TimeProvider timeProvider,
    IFileSystemWatcherRx fileSystemWatcherRx,
    IResilientFileSystem resilientFileSystem) : ILogfileGateway
{
    public async Task<OneOf<Logfile, Error>> UploadOperationAsync(
        Logfile logfileToUpload,
        int maxRetries,
        TimeSpan timeout,
        CancellationToken ct = default)
    {
        var retries = 0;
        OneOf<Logfile, Error> result;
        do
        {
            if (ct.IsCancellationRequested)
            {
                return Error.OperationCancelled;
            }

            if (retries > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(retries * 1), timeProvider, ct);
            }

            result = await UploadImplementationAsync(logfileToUpload, timeout, ct);
            retries++;
        } while (
            !ct.IsCancellationRequested &&
            result.IsT1 &&
            retries <= maxRetries);

        return result;
    }

    private async Task<OneOf<Logfile, Error>> UploadImplementationAsync(
        Logfile logfileToUpload,
        TimeSpan timeOut,
        CancellationToken ct)
    {
        try
        {
            fileSystemWatcherRx.Start(options.BaseDirectory);
            var resultTask = fileSystemWatcherRx.FileCreated
                .Where(x => IsResponseFile(x, logfileToUpload))
                .SelectAwait(CreateTextDocument)
                .FirstAsync(ct);

            var serverResponseTask = resultTask.WaitAsync(timeOut, timeProvider, ct);
            var sendLogfileTask = SendLogfileAsync(logfileToUpload, ct);

            await Task.WhenAll(serverResponseTask, sendLogfileTask);

            fileSystemWatcherRx.Stop();
            return serverResponseTask.Result;
        }
        catch (Exception e)
        {
            return new Error(e.Message);
        }
    }

    private async Task SendLogfileAsync(Logfile logfileToUpload, CancellationToken ct)
    {
        var destinationFullPath = Path.Combine(options.BaseDirectory.FullName, logfileToUpload.Name);
        if (logfileToUpload.Exists)
        {
            await resilientFileSystem.CopyFileAsync(
                logfileToUpload.FullName,
                destinationFullPath,
                ct);
        }
        else
        {
            await resilientFileSystem.WriteAllTextAsync(
                destinationFullPath,
                logfileToUpload.Content,
                ct);
        }
    }

    private bool IsResponseFile(string? fullPath, Logfile logfileToUpload)
    {
        if (fullPath == null) return false;
        var expectedFileName = Path.GetFileNameWithoutExtension(logfileToUpload.Name) +
                               options.ResponseExtensionType.GetDescription();
        return fullPath.EndsWith(expectedFileName);
    }

    private async ValueTask<Logfile> CreateTextDocument(string fullPath, CancellationToken ct)
    {
        var content = await resilientFileSystem.ReadAllTextAsync(fullPath, ct);
        return new Logfile()
        {
            Content = content,
            FileInfo = new FileInfo(fullPath)
        };
    }
}