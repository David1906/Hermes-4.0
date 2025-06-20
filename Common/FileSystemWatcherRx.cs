using R3;

namespace Common;

public class FileSystemWatcherRx : IDisposable, IFileSystemWatcherRx
{
    private readonly FileSystemWatcher _watcher = new();

    public ReactiveProperty<string> LastFileCreated { get; } = new("");

    private DisposableBag _disposableBag;

    public FileSystemWatcherRx()
    {
        Observable
            .FromEvent<FileSystemEventHandler, FileSystemEventArgs>(
                h => (_, e) => h(e),
                h => this._watcher.Created += h,
                h => this._watcher.Created -= h)
            .Subscribe(x => this.LastFileCreated.Value = x.FullPath)
            .AddTo(ref _disposableBag);
    }

    public void Start(DirectoryInfo directory, string filter = "*.*")
    {
        this.LastFileCreated.Value = null;
        this._watcher.Path = directory.FullName;
        this._watcher.Filter = filter;
        this._watcher.EnableRaisingEvents = true;
    }

    public void Stop()
    {
        this._watcher.EnableRaisingEvents = false;
    }

    public void Dispose()
    {
        this._watcher.Dispose();
        this._disposableBag.Dispose();
    }
}