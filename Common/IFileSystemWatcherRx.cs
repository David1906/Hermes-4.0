using R3;

namespace Common;

public interface IFileSystemWatcherRx
{
    Subject<string> FileCreated { get; }
    void Start(DirectoryInfo directory, string filter = "*.*");
    void Stop();
    void Dispose();
}