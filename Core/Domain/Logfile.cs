using Core.Domain.Common.Types;

namespace Core.Domain;

public class Logfile
{
    public int Id { get; init; }
    public required FileInfo FileInfo { get; set; }
    public LogfileType Type { get; set; }

    public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(this.FileInfo.FullName);
    public string FullName => this.FileInfo.FullName;
    public string Name => this.FileInfo.Name;
    public bool Exists => this.FileInfo.Exists;
}