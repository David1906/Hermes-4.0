namespace Common;

public class TextDocument
{
    public int Id { get; init; }
    public required string Content { get; set; }
    public required FileInfo FileInfo { get; set; }

    public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(this.FileInfo.FullName);
    public bool IsEmpty => string.IsNullOrEmpty(this.Content);
}