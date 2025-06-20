using Polly;
using Polly.Retry;

namespace Common;

public class ResilientFileSystem : IResilientFileSystem
{
    private readonly ResiliencePipeline _retryPipeline;

    public int DelayMilliseconds { get; set; } = 200;
    public int MaxDelayMilliseconds { get; set; } = 500;
    public int MaxRetryAttempts { get; set; } = 3;
    public int TimeOutMilliseconds { get; set; } = 10000;

    public ResilientFileSystem(TimeProvider timeProvider)
    {
        var builder = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions()
            {
                BackoffType = DelayBackoffType.Linear,
                Delay = TimeSpan.FromMilliseconds(this.DelayMilliseconds),
                MaxDelay = TimeSpan.FromMilliseconds(this.MaxDelayMilliseconds),
                MaxRetryAttempts = this.MaxRetryAttempts,
                UseJitter = true
            })
            .AddTimeout(TimeSpan.FromSeconds(this.TimeOutMilliseconds));
        builder.TimeProvider = timeProvider;
        _retryPipeline = builder.Build();
    }

    public async Task<string> ReadAllTextAsync(string fullPath, CancellationToken ct = default)
    {
        if (!File.Exists(fullPath))
        {
            return string.Empty;
        }

        return await _retryPipeline.ExecuteAsync(async x =>
            await File.ReadAllTextAsync(fullPath, x), ct);
    }

    public async Task<string> CopyFileAsync(
        string originFullPath,
        string destinationFullPath,
        CancellationToken ct = default)
    {
        return await _retryPipeline.ExecuteAsync(async ct1 =>
        {
            return await Task.Run(() =>
            {
                CreateDirectoryIfNotExists(destinationFullPath);
                File.Copy(originFullPath, destinationFullPath);
                return destinationFullPath;
            }, ct1);
        }, ct);
    }

    public async Task<string> MoveFileAsync(string originFullPath, string destinationFullPath,
        CancellationToken ct = default)
    {
        return await _retryPipeline.ExecuteAsync(async ct1 =>
        {
            return await Task.Run(() =>
            {
                CreateDirectoryIfNotExists(destinationFullPath);
                File.Move(originFullPath, destinationFullPath);
                return destinationFullPath;
            }, ct1);
        }, ct);
    }

    private void CreateDirectoryIfNotExists(string fullPath)
    {
        var path = Path.GetDirectoryName(fullPath);
        if (path != null && !Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public async Task<string> DeleteFileIfExistsAsync(string fullPath, CancellationToken ct = default)
    {
        return await _retryPipeline.ExecuteAsync(async ct1 =>
        {
            return await Task.Run(() =>
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                return fullPath;
            }, ct1);
        }, ct);
    }

    public async Task WriteAllTextAsync(string destinationFullPath, string content, CancellationToken ct)
    {
        CreateDirectoryIfNotExists(destinationFullPath);
        await File.WriteAllTextAsync(destinationFullPath, content, ct);
    }
}