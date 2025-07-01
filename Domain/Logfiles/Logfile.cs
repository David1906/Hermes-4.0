using Domain.Core.Types;

namespace Domain.Logfiles;

public class Logfile
{
    public int Id { get; init; }
    public string Content { get; set; } = "";
    public required FileInfo FileInfo { get; set; }
    public LogfileType Type { get; set; }

    public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(this.FileInfo.FullName);
    public bool IsEmpty => string.IsNullOrEmpty(this.Content);
    public string FullName => this.FileInfo.FullName;
    public string Name => this.FileInfo.Name;
    public bool Exists => this.FileInfo.Exists;
}