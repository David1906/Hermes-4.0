using Common.Extensions;
using Common.ResultOf;
using Common;
using Desktop.Features.Logfiles.Domain;
using R3;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Desktop.Gateways;

public class LogfilesSfcGateway(
    LogfilesGatewayOptions options,
    TimeProvider timeProvider,
    IFileSystemWatcherRx fileSystemWatcherRx,
    IResilientFileSystem resilientFileSystem) : ILogfilesSfcGateway
{
    public async Task<ResultOf<Logfile>> UploadOperationAsync(
        Logfile logfileToUpload,
        int maxRetries,
        TimeSpan timeout,
        CancellationToken ct = default)
    {
        var retries = 0;
        ResultOf<Logfile> result;
        do
        {
            if (ct.IsCancellationRequested)
            {
                return ResultOf<Logfile>.Failure(Error.OperationCancelled);
            }

            if (retries > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(retries * 1), timeProvider, ct);
            }

            result = await UploadImplementationAsync(logfileToUpload, timeout, ct);
            retries++;
        } while (
            !ct.IsCancellationRequested &&
            !result.IsSuccess &&
            retries <= maxRetries);

        return result;
    }

    private async Task<ResultOf<Logfile>> UploadImplementationAsync(
        Logfile logfileToUpload,
        TimeSpan timeOut,
        CancellationToken ct)
    {
        ResultOf<Logfile> result;
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

            result = serverResponseTask.Result;
        }
        catch (TimeoutException)
        {
            result = ResultOf<Logfile>.Failure(Error.Timeout);
        }
        catch (OperationCanceledException)
        {
            result = ResultOf<Logfile>.Failure(Error.OperationCancelled);
        }
        catch (Exception e)
        {
            result = ResultOf<Logfile>.Failure(e.Message);
        }
        finally
        {
            fileSystemWatcherRx.Stop();
        }

        return result;
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