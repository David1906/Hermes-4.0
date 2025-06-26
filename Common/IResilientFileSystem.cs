namespace Common;

public interface IResilientFileSystem
{
    Task<string> ReadAllTextAsync(string fullPath, CancellationToken ct = default);
    Task<string> CopyFileAsync(string originFullPath, string destinationFullPath, CancellationToken ct = default);
    Task<string> DeleteFileIfExistsAsync(string fullPath, CancellationToken ct = default);
    Task<string> MoveFileAsync(string originFullPath, string destinationFullPath, CancellationToken ct = default);
    Task WriteAllTextAsync(string destinationFullPath, string content, CancellationToken ct = default);
}