using Core.Domain;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.Features.Logfiles;

[Mapper]
public static partial class LogfileMappers
{
    [MapperIgnoreSource(nameof(Logfile.FileNameWithoutExtension))]
    [MapperIgnoreSource(nameof(Logfile.FullName))]
    [MapperIgnoreSource(nameof(Logfile.Name))]
    [MapperIgnoreSource(nameof(Logfile.Exists))]
    [MapProperty(nameof(Logfile.FileInfo), nameof(LogfileDto.FileInfo), Use = nameof(MapFileInfo))]
    public static partial LogfileDto ToDto(this Logfile logfile);

    private static string MapFileInfo(FileInfo fileInfo)
        => fileInfo.FullName;

    [MapProperty(nameof(LogfileDto.FileInfo), nameof(Logfile.FileInfo),
        Use = nameof(MapStringToFileInfo))]
    public static partial Logfile ToDomainModel(this LogfileDto logfileDto);

    private static FileInfo MapStringToFileInfo(string fullPath)
        => new FileInfo(fullPath);
}