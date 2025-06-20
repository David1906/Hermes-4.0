using R3;

namespace Common;

public interface IFileSystemWatcherRx
{
    ReactiveProperty<string> LastFileCreated { get; }
    void Start(DirectoryInfo directory, string filter = "*.*");
    void Stop();
    void Dispose();
}