using Common;
using Common.Extensions;
using Domain.Operations;
using R3;
using ROP;
using Result = ROP.Result;

namespace Data.Sfc;

public class SfcApi(
    SfcApiOptions options,
    TimeProvider timeProvider,
    IFileSystemWatcherRx fileSystemWatcherRx,
    IResilientFileSystem resilientFileSystem) : ISfcApi
{
    public async Task<Result<TextDocument>> UploadOperationAsync(
        AddOperationToSfcRequest request,
        CancellationToken ct = default)
    {
        var retries = 0;
        Result<TextDocument> result;
        do
        {
            if (ct.IsCancellationRequested)
            {
                return Result.Failure<TextDocument>("Operation cancelled by user.");
            }

            if (retries > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(retries * 1), timeProvider, ct);
            }

            result = await UploadImplementationAsync(request, ct);
            retries++;
        } while (
            !ct.IsCancellationRequested &&
            !result.Success &&
            retries <= request.MaxRetries);

        return result;
    }

    private async Task<Result<TextDocument>> UploadImplementationAsync(
        AddOperationToSfcRequest request,
        CancellationToken ct)
    {
        try
        {
            fileSystemWatcherRx.Start(options.BaseDirectory);
            await SendUnitUnderTest(request, ct);

            var resultTask = fileSystemWatcherRx.LastFileCreated
                .Where(x => IsResponseFile(x, request))
                .Delay(TimeSpan.FromMilliseconds(20))
                .SelectAwait(ReadTextDocument)
                .FirstAsync(ct);
            var result = await resultTask.WaitAsync(request.Timeout, timeProvider, ct);
            fileSystemWatcherRx.Stop();
            return result;
        }
        catch (Exception e)
        {
            return Result.Failure<TextDocument>(e.Message);
        }
    }

    private async Task SendUnitUnderTest(AddOperationToSfcRequest request, CancellationToken ct)
    {
        await resilientFileSystem.CopyFileAsync(
            request.FileToUpload.FullName,
            Path.Combine(options.BaseDirectory.FullName, request.FileToUpload.Name),
            ct);
    }

    private bool IsResponseFile(string? fullPath, AddOperationToSfcRequest request)
    {
        if (fullPath == null) return false;
        var expectedFileName = Path.GetFileNameWithoutExtension(request.FileToUpload.Name) +
                               options.ResponseExtensionType.GetDescription();
        return fullPath.EndsWith(expectedFileName);
    }

    private async ValueTask<TextDocument> ReadTextDocument(string fullPath, CancellationToken ct)
    {
        var content = await resilientFileSystem.ReadAllTextAsync(fullPath, ct);
        return new TextDocument()
        {
            Content = content,
            FileInfo = new FileInfo(fullPath)
        };
    }
}